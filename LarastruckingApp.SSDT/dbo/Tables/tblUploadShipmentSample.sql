CREATE TABLE [dbo].[tblUploadShipmentSample] (
    [SampleId]           INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]         INT            NULL,
    [ShipmentSamplePath] NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([SampleId] ASC)
);

