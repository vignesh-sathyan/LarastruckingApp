CREATE TABLE [dbo].[tblEquipmentDocument] (
    [DocumentId]         INT            IDENTITY (1, 1) NOT NULL,
    [EDID]               INT            NULL,
    [DocumentTypeId]     INT            NULL,
    [DocumentName]       NVARCHAR (100) NULL,
    [DocumentIssueDate]  DATETIME       NULL,
    [DocumentExpiryDate] DATETIME       NULL,
    [ImageURL]           NVARCHAR (MAX) NULL,
    [ImageName]          NVARCHAR (200) NULL,
    [IsActive]           BIT            NULL,
    [CreatedDate]        DATETIME       NULL,
    [CreatedBy]          INT            NOT NULL,
    [ModifiedDate]       DATETIME       NULL,
    [ModifiedBy]         INT            NOT NULL,
    [IsDeleted]          BIT            NOT NULL,
    CONSTRAINT [PK_tblEquipmentDocument] PRIMARY KEY CLUSTERED ([DocumentId] ASC)
);

