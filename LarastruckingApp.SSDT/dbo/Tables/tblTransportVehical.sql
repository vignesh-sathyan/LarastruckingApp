CREATE TABLE [dbo].[tblTransportVehical] (
    [TransportVehicalID]    BIGINT   IDENTITY (1, 1) NOT NULL,
    [VehicleType]           INT      NULL,
    [VehicleStatus]         INT      NULL,
    [PreTripCheckup_Tires]  BIT      NOT NULL,
    [PreTripCheckup_Breaks] BIT      NOT NULL,
    [PreTripCheckup_fuel]   BIT      NOT NULL,
    [CheckupDate]           DATETIME NULL,
    [CheckupBy]             BIGINT   NULL,
    [CreatedBy]             BIGINT   NULL,
    [CreatedOn]             DATETIME NULL,
    [ModifiedBy]            BIGINT   NULL,
    [ModifiedOn]            DATETIME NULL,
    [IsDeleted]             BIT      NULL,
    CONSTRAINT [PK_tblTransportVehical] PRIMARY KEY CLUSTERED ([TransportVehicalID] ASC),
    CONSTRAINT [FK_tblTransportVehical_tblVehicleStatus] FOREIGN KEY ([VehicleStatus]) REFERENCES [dbo].[tblEquipmentStatus] ([VehicleStatusID])
);

