CREATE TABLE [portal].[IncidentHistory]
(
    [Id]           BIGINT IDENTITY (1, 1)             NOT NULL,
    [OperationId]  BIGINT                             NOT NULL,
    [ChangedBy]    INT                                NULL,
    [ChangeTime]   DATETIME2(3) DEFAULT SYSDATETIME() NOT NULL,
    [AuthorId]     INT                                NULL,
    [AssigneeId]   INT                                NULL,
    [IsUnassigned] BIT                                NULL,
    [StatusId]     TINYINT                            NULL,
    [Comment]      NVARCHAR(255)                      NULL,
    CONSTRAINT [PK_IncidentHistory_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IncidentHistory_OperationId] FOREIGN KEY ([OperationId]) REFERENCES [portal].[Incident] ([OperationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_IncidentHistory_ChangedBy] FOREIGN KEY ([ChangedBy]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_IncidentHistory_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_IncidentHistory_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_IncidentHistory_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [enum].[IncidentStatus] ([Id])
);