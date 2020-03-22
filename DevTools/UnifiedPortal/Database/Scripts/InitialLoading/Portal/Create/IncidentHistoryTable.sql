CREATE TABLE [portal].[IncidentHistory]
(
    [OperationId]  BIGINT                          NOT NULL,
    [ChangeTime]   DATETIME2 DEFAULT SYSDATETIME() NOT NULL,
    [AuthorId]     INT                             NULL,
    [AssigneeId]   INT                             NULL,
    [IsUnassigned] BIT                             NULL,
    [StatusId]     TINYINT                         NULL,
    [Comment]      NVARCHAR(255)                   NULL,
    CONSTRAINT [PK_IncidentHistory_OperationId] PRIMARY KEY CLUSTERED ([OperationId] ASC),
    CONSTRAINT [FK_IncidentHistory_OperationId] FOREIGN KEY ([OperationId]) REFERENCES [portal].[Incident] ([OperationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_IncidentHistory_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_IncidentHistory_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_IncidentHistory_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [enum].[IncidentStatus] ([Id])
);