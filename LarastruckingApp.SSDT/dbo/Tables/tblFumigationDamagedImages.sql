CREATE TABLE [dbo].[tblFumigationDamagedImages] (
    [DamagedID]         INT           IDENTITY (1, 1) NOT NULL,
    [FumigationRouteId] INT           NULL,
    [ImageName]         VARCHAR (MAX) NULL,
    [ImageDescription]  VARCHAR (MAX) NULL,
    [ImageUrl]          VARCHAR (MAX) NULL,
    [CreatedBy]         INT           NOT NULL,
    [CreatedOn]         DATETIME      NOT NULL,
    [IsDeleted]         BIT           NOT NULL,
    [DeletedOn]         DATETIME      NULL,
    [DeletedBy]         INT           NULL,
    [IsApproved]        BIT           CONSTRAINT [DF_tblFumigationDamagedImages_IsApproved] DEFAULT ((0)) NOT NULL,
    [ApprovedBy]        INT           NULL,
    [ApprovedOn]        DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([DamagedID] ASC),
    CONSTRAINT [FK__tblFumiga__Fumig__697C9932] FOREIGN KEY ([FumigationRouteId]) REFERENCES [dbo].[tblFumigationRouts] ([FumigationRoutsId])
);







