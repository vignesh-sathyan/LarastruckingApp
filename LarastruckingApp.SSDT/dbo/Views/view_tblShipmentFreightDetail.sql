CREATE VIEW view_tblShipmentFreightDetail AS
 
 select SFD.ShipmentBaseFreightDetailId, SFD.IsDeleted, SFD.IsPartialShipment, SFD.ShipmentId,SFD.ShipmentRouteStopeId,SFD.QuantityNweight,SFD.NoOfBox,SFD.Weight,SFD.Unit,SFD.PartialPallete,SFD.PartialBox from tblShipmentFreightDetail SFD WHERE IsDeleted=0