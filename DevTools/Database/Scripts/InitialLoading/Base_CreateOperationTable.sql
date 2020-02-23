CREATE TABLE [base].[Operation]
(
    [Id]             BIGINT IDENTITY (1, 1)         NOT NULL,
    [Scope]          NVARCHAR(16)                   NOT NULL,
    [ContextName]    NVARCHAR(512)                  NOT NULL,
    [UserId]         INT                            NULL,
    [IsSystemAction] BIT      DEFAULT ((0))         NOT NULL,
    [IsSuccessful]   BIT                            NULL,
    [StartTime]      DATETIME DEFAULT SYSDATETIME() NOT NULL,
    [StopTime]       DATETIME                       NULL,
    CONSTRAINT [PK_Operation_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Operation_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id]) ON DELETE CASCADE
);