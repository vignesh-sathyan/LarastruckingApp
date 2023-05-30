CREATE TABLE [dbo].[tblShipmentStatus] (
    [StatusId]                       INT           IDENTITY (1, 1) NOT NULL,
    [StatusName]                     VARCHAR (100) NULL,
    [DisplayOrder]                   INT           NOT NULL,
    [IsActive]                       BIT           NOT NULL,
    [IsDeleted]                      BIT           NOT NULL,
    [Colour]                         VARCHAR (100) NULL,
    [ImageURL]                       VARCHAR (MAX) NULL,
    [FumigationDisplayOrder]         INT           NULL,
    [IsFumigation]                   BIT           NULL,
    [IsShipment]                     BIT           NULL,
    [StatusAbbreviation]             VARCHAR (100) NULL,
    [FontColor]                      VARCHAR (20)  NULL,
    [DisplayOrderCustomer]           INT           NULL,
    [FumigationDisplayOrderCustomer] INT           NULL,
    [GrayImageURL]                   VARCHAR (MAX) NULL,
    [SpanishStatusAbbreviation]      VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([StatusId] ASC)
);


























GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_tblShipmentStatus]
    ON [dbo].[tblShipmentStatus]([StatusId] ASC, [StatusName] ASC);

