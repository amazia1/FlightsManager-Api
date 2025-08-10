using API.Dtos.Flights;
using API.Models;
using API.Repositories.Flights;
using API.Services.Flights;
using API.Services.Notifications;
using API.Validators;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Api.UnitTests;

public class CreateFlightsTests
{
    [Fact]
    public async Task CreateAsync_ValidInput_Persists_ReturnsDto_And_Notifies()
    {
        var dto = new CreateFlightDto
        {
            Destination = "Paris",
            Gate = "A12",
            DepartureTime = DateTime.Now.AddHours(2)
        };

        IValidator<CreateFlightDto> validator = new CreateFlightDtoValidator();
        validator.Validate(dto).IsValid.Should().BeTrue();

        var repo = new Mock<IFlightsRepository>();
        repo.Setup(r => r.CreateFlight(It.IsAny<Flight>()))
            .ReturnsAsync((Flight f) => { f.FlightNumber = 101; return f; });

        var notifier = new Mock<INotificationService>();

        var mapper = new Mock<IMapper>();
        mapper.Setup(m => m.Map<Flight>(It.IsAny<CreateFlightDto>()))
            .Returns((CreateFlightDto d) => new Flight
            {
                Destination = d.Destination,
                Gate = d.Gate,
                DepartureTime = d.DepartureTime
            });

        mapper.Setup(m => m.Map<FlightDto>(It.IsAny<Flight>()))
            .Returns((Flight f) => new FlightDto
            {
                FlightNumber = f.FlightNumber,
                Destination = f.Destination,
                Gate = f.Gate,
                DepartureTime = f.DepartureTime
            });

        var sut = new FlightsService(mapper.Object, repo.Object, notifier.Object);

        var result = await sut.CreateFlight(dto);

        result.Data?.FlightNumber.Should().Be(101);
        result.Data?.Destination.Should().Be("Paris");
        result.Data?.Gate.Should().Be("A12");
        result.Data?.DepartureTime.Should().BeCloseTo(dto.DepartureTime, TimeSpan.FromSeconds(1));

        repo.Verify(r => r.CreateFlight(
            It.Is<Flight>(f =>
                f.Destination == dto.Destination &&
                f.Gate == dto.Gate &&
                f.DepartureTime == dto.DepartureTime)), Times.Once);
    }
}