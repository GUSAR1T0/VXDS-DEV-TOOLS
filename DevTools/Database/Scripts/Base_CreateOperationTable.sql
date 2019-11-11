CREATE TABLE [base].[Operation] (
    [Id]             BIGINT         IDENTITY (1, 1),
    [Scope]          NVARCHAR (16)  NOT NULL,
    [ContextName]    NVARCHAR (512) NOT NULL,
    [UserId]         INT            NULL,
    [IsSystemAction] BIT            DEFAULT ((0)) NOT NULL,
    [IsSuccessful]   BIT            NULL,
    [StartDate]      DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET() NOT NULL,
    [StopDate]       DATETIMEOFFSET NULL,
    CONSTRAINT [PK_Operation_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Operation_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id]) ON DELETE CASCADE
);