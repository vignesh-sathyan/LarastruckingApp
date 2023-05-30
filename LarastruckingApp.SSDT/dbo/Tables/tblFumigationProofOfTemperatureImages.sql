CREATE TABLE [dbo].[tblFumigationProofOfTemperatureImages] (
    [ImageId]           INT           IDENTITY (1, 1) NOT NULL,
    [ImageName]         VARCHAR (MAX) NULL,
    [ImageDescription]  VARCHAR (MAX) NULL,
    [ImageUrl]          VARCHAR (MAX) NULL,
    [FumigationRouteId] INT           NULL,
    [ActualTemperature] VARCHAR (50)  NULL,
    [CreatedBy]         INT           NOT NULL,
    [CreatedOn]         DATETIME      NOT NULL,
    [IsDeleted]         BIT           NOT NULL,
    [DeletedOn]         DATETIME      NULL,
    [DeletedBy]         INT           NULL,
    [IsApproved]        BIT           CONSTRAINT [DF_tblFumigationProofOfTemperatureImages_IsApproved] DEFAULT ((0)) NOT NULL,
    [ApprovedBy]        INT           NULL,
    [ApprovedOn]        DATETIME      NULL,
    [IsLoading]         BIT           CONSTRAINT [DF_tblFumigationProofOfTemperatureImages_IsLoading] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__tblFumig__7516F70CF00299D3] PRIMARY KEY CLUSTERED ([ImageId] ASC),
    CONSTRAINT [FK__tblFumiga__Fumig__66A02C87] FOREIGN KEY ([FumigationRouteId]) REFERENCES [dbo].[tblFumigationRouts] ([FumigationRoutsId])
);











