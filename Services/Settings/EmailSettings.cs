namespace Services.Settings;

public record EmailSettings(
    string EmailAddressDisplay,
    string EmailAddress,
    string SmtpServerAddress,
    int SmtpServerPort,
    string SmtpUserName,
    string SmtpPassword,
    bool EnableSSL,
    string EnvironmentSubjectPrefix
);



