using Microsoft.Extensions.Logging;
using Moq;

namespace Clamify.Tests.TestUtilities;

/// <summary>
/// Helper class to verify a log was preformed for various log levels.
/// </summary>
public static class LoggerCallHelpers
{
    /// <summary>
    /// Ensure the logger logged a debug entry.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    public static void VerifyDebugCall(this ILogger logger) =>
        VerifyDebugCall(logger, Times.Once());

    /// <summary>
    /// Ensure the logger logged a debug entry a specified number of times.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    /// <param name="times">The number of times the logger should have been called.</param>
    public static void VerifyDebugCall(this ILogger logger, Times times) =>
        VerifyCall(logger, times, LogLevel.Debug);

    /// <summary>
    /// Ensure the logger logged a information entry.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    public static void VerifyInformationCall(this ILogger logger) =>
        VerifyInformationCall(logger, Times.Once());

    /// <summary>
    /// Ensure the logger logged a information entry a specified number of times.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    /// <param name="times">The number of times the logger should have been called.</param>
    public static void VerifyInformationCall(this ILogger logger, Times times) =>
        VerifyCall(logger, times, LogLevel.Information);

    /// <summary>
    /// Ensure the logger logged a warning entry.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    public static void VerifyWarningCall(this ILogger logger) =>
        VerifyWarningCall(logger, Times.Once());

    /// <summary>
    /// Ensure the logger logged a warning entry a specified number of times.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    /// <param name="times">The number of times the logger should have been called.</param>
    public static void VerifyWarningCall(this ILogger logger, Times times) =>
        VerifyCall(logger, times, LogLevel.Warning);

    /// <summary>
    /// Ensure the logger logged an error entry.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    public static void VerifyErrorCall(this ILogger logger) =>
        VerifyErrorCall<Exception>(logger, Times.Once());

    /// <summary>
    /// Ensure the logger logged a warning entry a specified number of times.
    /// </summary>
    /// <param name="logger">The logger object to verify.</param>
    /// <param name="times">The number of times the logger should have been called.</param>
    public static void VerifyErrorCall(this ILogger logger, Times times) =>
        VerifyErrorCall<Exception>(logger, times);

    /// <summary>
    /// Ensure the logger logged an error entry for a specific exception.
    /// </summary>
    /// <typeparam name="TException">The type of exception to verify was logged.</typeparam>
    /// <param name="logger">The logger object to verify.</param>
    public static void VerifyErrorCall<TException>(this ILogger logger)
        where TException : Exception =>
        VerifyErrorCall<TException>(logger, Times.Once());

    /// <summary>
    /// Ensure the logger logged a warning entry for a specific exception a specified number of times.
    /// </summary>
    /// <typeparam name="TException">The type of exception to verify was logged.</typeparam>
    /// <param name="logger">The logger object to verify.</param>
    /// <param name="times">The number of times the logger should have been called.</param>
    public static void VerifyErrorCall<TException>(this ILogger logger, Times times)
        where TException : Exception =>
        VerifyCall<TException>(logger, times, LogLevel.Error);

    private static void VerifyCall(this ILogger logger, Times times, LogLevel logLevel) =>
        VerifyCall<Exception>(logger, times, logLevel);

    private static void VerifyCall<TException>(this ILogger logger, Times times, LogLevel logLevel)
        where TException : Exception =>
        Mock.Get(logger)
            .Verify(
                m =>
                    m.Log(
                        logLevel,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) => true),
                        It.IsAny<TException>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times);
}
