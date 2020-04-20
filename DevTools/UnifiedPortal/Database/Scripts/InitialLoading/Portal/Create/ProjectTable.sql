CREATE TABLE [portal].[Project]
(
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR(255)       NOT NULL,
    [Alias]        VARCHAR(30)         NOT NULL,
    [Description]  NVARCHAR(1024)      NULL,
    [GitHubRepoId] BIGINT              NULL,
    [IsActive]     BIT DEFAULT ((1))   NOT NULL,
    CONSTRAINT [PK_Project_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Project_Name] UNIQUE ([Name] ASC),
    CONSTRAINT [UQ_Project_Alias] UNIQUE ([Alias] ASC)
);