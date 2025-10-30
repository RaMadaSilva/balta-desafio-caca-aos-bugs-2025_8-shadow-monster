using BugStore.Domain.Exceptions;
using FluentAssertions;

namespace BugStore.Domain.Tests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void NotFoundException_Should_BeInstanceOfException()
    {
        // Arrange & Act
        var exception = new NotFoundException();

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public void NotFoundException_WithMessage_Should_HaveMessage()
    {
        // Arrange & Act
        var message = "Resource not found";
        var exception = new NotFoundException(message);

        // Assert
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void NotFoundException_WithMessageAndInnerException_Should_HaveBoth()
    {
        // Arrange
        var message = "Resource not found";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new NotFoundException(message, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }
}

