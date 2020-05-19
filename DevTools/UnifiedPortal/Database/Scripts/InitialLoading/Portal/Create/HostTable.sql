CREATE TABLE [portal].[Host]
(
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR(50)         NOT NULL,
    [Domain]            NVARCHAR(512)       NOT NULL,
    [OperatingSystemId] TINYINT             NOT NULL,
    [Credentials]       NVARCHAR(MAX)       NULL,
    [IsActive]          BIT DEFAULT (1)     NOT NULL,
    CONSTRAINT [PK_Host_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Host_Name] UNIQUE ([Name] ASC),
    CONSTRAINT [FK_IncidentHistory_OperatingSystemId] FOREIGN KEY ([OperatingSystemId]) REFERENCES [enum].[OperatingSystem] ([Id]) ON DELETE CASCADE
);