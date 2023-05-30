CREATE TABLE [dbo].[tblShipmentEventHistory] (
    [ID]                      INT           IDENTITY (1, 1) NOT NULL,
    [ShipmentId]              INT           NULL,
    [ShipmentRouteStopId]     INT           NULL,
    [ShipmentFreightDetailId] INT           NULL,
    [StatusId]                INT           NULL,
    [UserId]                  INT           NULL,
    [Event]                   VARCHAR (50)  NULL,
    [EventDetail]             VARCHAR (200) NULL,
    [EventDateTime]           DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

