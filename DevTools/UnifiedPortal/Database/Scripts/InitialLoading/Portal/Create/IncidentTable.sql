CREATE TABLE [portal].[Incident]
(
    [OperationId] BIGINT                             NOT NULL,
    [InitialTime] DATETIME2(3) DEFAULT SYSDATETIME() NOT NULL,
    [AuthorId]    INT                                NOT NULL,
    [AssigneeId]  INT                                NULL,
    [StatusId]    TINYINT      DEFAULT ((1))         NOT NULL,
    CONSTRAINT [PK_Incident_OperationId] PRIMARY KEY CLUSTERED ([OperationId] ASC),
    CONSTRAINT [FK_Incident_OperationId] FOREIGN KEY ([OperationId]) REFERENCES [base].[Operation] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Incident_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_Incident_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_Incident_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [enum].[IncidentStatus] ([Id]) ON DELETE CASCADE
);