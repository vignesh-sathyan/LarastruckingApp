CREATE TABLE [dbo].[tblRelationships] (
    [ContactRelationshipId] INT          NOT NULL,
    [ContactRelationship]   VARCHAR (50) NULL,
    CONSTRAINT [PK_Relationships] PRIMARY KEY CLUSTERED ([ContactRelationshipId] ASC)
);

