CREATE TABLE [base].[File]
(
    [Id]          INT IDENTITY (1, 1)                  NOT NULL,
    [Name]        NVARCHAR(255)                        NOT NULL,
    [ExtensionId] TINYINT                              NOT NULL,
    [Content]     VARBINARY(MAX)                       NOT NULL,
    [Time]        DATETIME2(3) DEFAULT (SYSDATETIME()) NOT NULL,
    [Hash]        NVARCHAR(40)                         NOT NULL,
    CONSTRAINT [PK_File_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_File_ExtensionId] FOREIGN KEY ([ExtensionId]) REFERENCES [enum].[FileExtension] ([Id]) ON DELETE CASCADE
);