using Microsoft.Extensions.Logging;
using Moq;

namespace Clamify.Tests.TestUtilities;

/// <summary>
/// Helper class to verify a log was preformed for various log levels.
/// </summary>
public static class LoggerCallHelpers
{
    public static void VerifyDebugCall(this ILogger logger) =>
        VerifyDebugCall(logger, Times.Once());

    public static void VerifyDebugCall(this ILogger logger, Times times) =>
        VerifyCall(logger, times, LogLevel.Debug);

    public static void VerifyInformationCall(this ILogger logger) =>
        VerifyInformationCall(logger, Times.Once());

    public static void VerifyInformationCall(this ILogger logger, Times times) =>
        VerifyCall(logger, times, LogLevel.Information);

    public static void VerifyWarningCall(this ILogger logger) =>
        VerifyWarningCall(logger, Times.Once());

    public static void VerifyWarningCall(this ILogger logger, Times times) =>
        VerifyCall(logger, times, LogLevel.Warning);

    public static void VerifyErrorCall(this ILogger logger) =>
        VerifyErrorCall<Exception>(logger, Times.Once());

    public static void VerifyErrorCall(this ILogger logger, Times times) =>
        VerifyErrorCall<Exception>(logger, times);

    public static void VerifyErrorCall<TException>(this ILogger logger)
        where TException : Exception =>
        VerifyErrorCall<TException>(logger, Times.Once());

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
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                    ),
                times
            );
}
