CREATE TABLE [portal].[Settings]
(
    [Id]    INT IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR(50)         NOT NULL,
    [Value] NVARCHAR(MAX)       NULL,
    CONSTRAINT [PK_PortalSettings_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_PortalSettings_Name] UNIQUE ([Name] ASC)
);