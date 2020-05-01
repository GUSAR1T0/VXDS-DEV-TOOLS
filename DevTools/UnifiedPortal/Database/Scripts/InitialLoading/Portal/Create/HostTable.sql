CREATE TABLE [portal].[Host]
(
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR(50)         NOT NULL,
    [Domain]            NVARCHAR(MAX)       NOT NULL,
    [OperationSystemId] TINYINT             NOT NULL,
    [Credentials]       NVARCHAR(MAX)       NULL,
    [IsActive]          BIT DEFAULT (1)     NOT NULL,
    CONSTRAINT [PK_Host_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Host_Name] UNIQUE ([Name] ASC),
    CONSTRAINT [UQ_Host_Domain] UNIQUE ([Domain] ASC),
    CONSTRAINT [FK_IncidentHistory_OperationSystemId] FOREIGN KEY ([OperationSystemId]) REFERENCES [enum].[OperationSystem] ([Id]) ON DELETE CASCADE
);