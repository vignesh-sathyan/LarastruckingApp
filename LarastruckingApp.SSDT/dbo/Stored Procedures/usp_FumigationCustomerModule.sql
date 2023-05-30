CREATE PROCEDURE [dbo].[usp_FumigationCustomerModule]
(@SpType            INT = NULL, 
 @FumigationId      INT = NULL, 
 @FumigationRoutsId INT = NULL
)
AS
    BEGIN

        --------------------------------------------------------------------------------------------------------------------------------                                     
        -- Multiple Routes Info basis of Fumigation and Cutomer                                    
        IF(@SpType = 1)
            BEGIN
                SET NOCOUNT ON;

                --;with CTE_TotalBoxNPallentCount(FumigationId, QuantityNMethod)                        
                --      AS (SELECT DISTINCT (FumigationId), CONCAT(SUM(FR.BoxCount),', ',SUM(FR.PalletCount) ) as QuantityNMethod                      
                --       FROM tblFumigationRouts FR                        
                --       GROUP BY FR.FumigationId),        
                --;with CTE_TotalBoxNPallentCount(FumigationId, QuantityNMethod)                        
                --      AS (SELECT DISTINCT (FumigationId), CONCAT(FR.BoxCount,' ',tpm.PricingMethodName) as QuantityNMethod                      
                --       FROM tblFumigationRouts FR        
                -- Left JOIN tblPricingMethod tpm ON tpm.PricingMethodId = FR.PricingMethod                         
                --      ),       
                --;with CTE_TotalBoxNPallentCount(FumigationId, QuantityNMethod)                        
                --      AS (SELECT DISTINCT (FumigationId), CONVERT(varchar(10),FR.BoxCount)  + ' ' +'Box' + ',' + CONVERT(varchar(10),FR.PalletCount) + ' ' + 'Pallet'  as QuantityNMethod                      
                --       FROM tblFumigationRouts FR       
                --       ),     

                WITH CTE_TotalBoxNPallentCount(FumigationRoutsIds, 
                                               QuantityNMethod)
                     AS (SELECT DISTINCT
                                (FumigationRoutsId), 
                                CONVERT(VARCHAR(MAX), concat(replace(CAST(SUM(FR.BoxCount) AS VARCHAR), '.00', ''), ' ' + 'Box' + ',' + CONVERT(VARCHAR(MAX), concat(replace(CAST(SUM(FR.PalletCount) AS VARCHAR), '.00', ''), ' ' + 'Pallet')))) AS QuantityNMethod
                         FROM tblFumigationRouts FR
                         GROUP BY FR.FumigationRoutsId),

                     CTE_PickUpEquipment(FumigationRoutsIds, 
                                         PickUpEquipment)
                     AS (SELECT DISTINCT 
                                FumigationRoutsId, 
                                STUFF(
                         (
                             SELECT ',' + ted.EquipmentNo
                             FROM tblEquipmentDetail ted
                                  JOIN tblFumigationEquipmentNDriver FEM ON ted.EDID = FEM.EquipmentId
                                                                            AND FEM.FumigationRoutsId = FUM.FumigationRoutsId    
																			AND FEM.IsDeleted=0            
                             -- JOIN tblDriver tu ON  tu.DriverID = FEM.DriverId              
                             -- WHERE tu.UserId=@UserId              
                             GROUP BY ted.EquipmentNo FOR XML PATH('')
                         ), 1, 1, '') AS PickUpEquipment
                         FROM tblFumigationEquipmentNDriver FUM
						 	WHERE  FUM.IsDeleted=0   
						 ),

                     cte_getDriver(FumigationRoutsIds, 
                                   DriverName)
                     AS (SELECT TS.FumigationRoutsId, 
                                STUFF(
                         (
                             SELECT DISTINCT 
                                    ', ' + CONVERT(VARCHAR(MAX), concat(TD.FirstName, ' ', TD.LastName))
                             FROM tblFumigationEquipmentNDriver TSED
                                  INNER JOIN tblDriver TD ON TSED.DriverId = TD.DriverID
                             WHERE TSED.FumigationRoutsId = TS.FumigationRoutsId FOR XML PATH('')
                         ), 1, 1, '') AS DriverName
                         FROM tblFumigationRouts TS)
						 
                     SELECT DISTINCT 
                            RS.RouteNo RouteOrder, 
                            RS.FumigationId, 
                            TS.CustomerId, 
                            RS.FumigationRoutsId, 
                            RS.AirWayBill, 
                            RS.CustomerPO, 
                            TBC.QuantityNMethod, 
                            CTEDR.DriverName, 
                            ISNULL(PE.PickUpEquipment, '') AS DriverEquipment,         
                            -- CONCAT(TD.FirstName ,'', TD.LastName) DriverName,                             
                            PickupLocation, 
                            CONCAT(A1.CompanyName, ', ', A1.Address1 + ' ' + A1.City + ' ' + S1.Name + ' ' + CONVERT(VARCHAR(200), A1.Zip) + ' ' + C1.Name) PickupAddress, 
                            A1.City PickupCity, 
                            S1.Name PickupState, 
                            C1.Name PickupCountry, 
                            PickUpArrival PickupDateTime, 
                            DeliveryLocation, 
                            CONCAT(A2.CompanyName, ', ', A2.Address1 + ' ' + A2.City + ' ' + S2.Name + ' ' + CONVERT(VARCHAR(200), A2.Zip) + ' ' + C2.Name) DeliveryAddress, 
                            A2.City DeliveryCity, 
                            S2.Name DeliveryState, 
                            C2.Name DeliveryCountry, 
                            DeliveryArrival DeliveryDateTime, 
                            RS.FumigationSite, 
                            CONCAT(A3.CompanyName, ', ', A3.Address1 + ' ' + A3.City + ' ' + S3.Name + ' ' + CONVERT(VARCHAR(200), A3.Zip) + ' ' + C3.Name) FumigationAddress, 
                            FumigationArrival FumigationDateTime           
                     -- TFEND.IsPickUp                            

                     FROM [dbo].[tblFumigationRouts] RS
                          LEFT JOIN [dbo].[tblFumigationEquipmentNDriver] TFEND ON TFEND.FumigationId = RS.FumigationId         
                          -- Left Join [dbo].[tblDriver] TD  ON TD.DriverID = TFEND.DriverId                                  
                          INNER JOIN [dbo].[tblFumigation] TS ON TS.FumigationId = RS.FumigationId
                          INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TS.CustomerId
                          INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = RS.PickupLocation
                          INNER JOIN [dbo].[tblState] S1 ON S1.ID = A1.State
                          INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country
                          INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = RS.DeliveryLocation
                          INNER JOIN [dbo].[tblState] S2 ON S2.ID = A2.State
                          INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country
                          LEFT JOIN [dbo].[tblAddress] A3 ON A3.AddressId = RS.FumigationSite
                          LEFT JOIN [dbo].[tblState] S3 ON S3.ID = A3.State
                          LEFT JOIN [dbo].[tblCountry] C3 ON C3.ID = A3.Country
                          LEFT JOIN CTE_PickUpEquipment PE ON RS.FumigationRoutsId = PE.FumigationRoutsIds
                          LEFT JOIN CTE_TotalBoxNPallentCount TBC ON RS.FumigationRoutsId = TBC.FumigationRoutsIds
                          LEFT JOIN cte_getDriver CTEDR ON RS.FumigationRoutsId = CTEDR.FumigationRoutsIds
                     WHERE RS.FumigationId = @FumigationId
                     ORDER BY RouteOrder;
        END                                    
                -------------------------------------------------------------------------------                                   
                -- Get Pre Trip Shipping Detail Based on Route Id                                                        ;
            ELSE
            IF(@SpType = 2)
                BEGIN
                    SET NOCOUNT ON;
                    WITH cte_getStatus
                         AS (SELECT SS.StatusName, 
                                    SH.StatusId, 
                                    SH.FumigationId
                             FROM tblFumigationStatusHistory SH
                                  INNER JOIN
                             (
                                 SELECT MAX(CreatedOn) AS MaxDate, 
                                        FumigationId
                                 FROM tblFumigationStatusHistory
                                 GROUP BY FumigationId
                             ) ssh ON SH.FumigationId = ssh.FumigationId
                                      AND SH.CreatedOn = ssh.MaxDate
                                  INNER JOIN tblShipmentStatus SS ON SH.StatusId = SS.StatusId)
                         SELECT TS.FumigationId, 
                                TSRS.FumigationRoutsId, 
                                TS.ShipmentRefNo, 
                                TSRS.AirWayBill, 
                                TSRS.CustomerPO, 
                                TSRS.ContainerNo, 
                                CONCAT(A1.CompanyName, ', ', A1.Address1 + ' ' + A1.City + ' ' + S1.Name + ' ' + CONVERT(VARCHAR(200), A1.Zip) + ' ' + C1.Name) PickUpLocation, 
                                CONCAT(A2.CompanyName, ', ', A2.Address1 + ' ' + A2.City + ' ' + S2.Name + ' ' + CONVERT(VARCHAR(200), A2.Zip) + ' ' + C2.Name) DeliveryAddress, 
                                CONCAT(A3.CompanyName, ', ', A3.Address1 + ' ' + A3.City + ' ' + S3.Name + ' ' + CONVERT(VARCHAR(200), A3.Zip) + ' ' + C3.Name) FumigationAddress, 
                                FumigationArrival FumigationDateTime, 
                                A1.Phone AS PickUpPhone, 
                                A1.Extension AS PickUpExtension, 
                                A2.Phone AS DeliveryPhone, 
                                A2.Extension AS DeliveryExtension, 
                                A3.Phone AS FumigationPhone, 
                                A3.Extension AS FumigationExtension, 
                                TSRS.DigitalSignature, 
                                TSRS.ReceiverName,                          
                                --For Customer Status and sub-Status ---     
                                CTES.StatusName AS StatusName,                           
                                -- TSS.StatusName,                              
                                TSSS.SubStatusName, 
                                TSSH.Reason, 
                                TSSH.StatusId, 
                                TSSH.SubStatusId                     
                         --------------------------------------                     

                         FROM [dbo].[tblFumigation] TS
                              INNER JOIN [dbo].[tblFumigationRouts] TSRS ON TSRS.FumigationId = TS.FumigationId
                              INNER JOIN [dbo].[tblFumigationStatusHistory] TSSH ON TSSH.FumigationId = TS.FumigationId
                              INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TSSH.StatusId
                              LEFT JOIN [dbo].[tblShipmentSubStatus] TSSS ON TSSS.SubStatusId = TSSH.SubStatusId
                              INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TSRS.PickupLocation
                              INNER JOIN [dbo].[tblState] S1 ON S1.ID = A1.State
                              INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country
                              INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TSRS.DeliveryLocation
                              INNER JOIN [dbo].[tblState] S2 ON S2.ID = A2.State
                              INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country
                              LEFT JOIN [dbo].[tblAddress] A3 ON A3.AddressId = TSRS.FumigationSite
                              LEFT JOIN [dbo].[tblState] S3 ON S3.ID = A3.State
                              LEFT JOIN [dbo].[tblCountry] C3 ON C3.ID = A3.Country
                              INNER JOIN cte_getStatus CTES ON TS.FumigationId = CTES.FumigationId
                         WHERE TSRS.FumigationRoutsId = @FumigationRoutsId
                               AND TS.IsDeleted = 0
                               AND TSRS.IsDeleted = 0;
            END                                                        
                    -----------------------------------------------------------------------------------------------                                                                                            
                    -- Bind the Damaged Files                                                               ;
                ELSE
                IF(@SpType = 3)
                    BEGIN
                        SET NOCOUNT ON;
                        SELECT TFDI.DamagedID, 
                               TFDI.FumigationRouteId, 
                               ISNULL(TFDI.ImageName, '') DamagedImage, 
                               ISNULL(TFDI.ImageDescription, '') DamagedDescription, 
                               ISNULL(TFDI.ImageUrl, '') ImageUrl, 
                               TFDI.CreatedOn DamagedDate
                        FROM [dbo].[tblFumigationDamagedImages] TFDI
                        WHERE TFDI.FumigationRouteId = @FumigationRoutsId
                              AND TFDI.IsApproved = 1
                              AND TFDI.IsDeleted = 0;
                END                                                              
                        ---------------------------------------------------------------------------------------------------                                                              ;
                    ELSE
                    IF(@SpType = 4)
                        BEGIN
                            SET NOCOUNT ON;
                            SELECT TFPOTI.ImageId proofImageId, 
                                   TFPOTI.FumigationRouteId, 
                                   ISNULL(TFPOTI.ImageName, '') ProofImage, 
                                   ISNULL(TFPOTI.ImageDescription, '') ProofDescription, 
                                   TFPOTI.ActualTemperature proofActualTemp, 
                                   ISNULL(TFPOTI.ImageUrl, '') ImageUrl, 
                                   TFPOTI.CreatedOn ProofDate
                            FROM [dbo].[tblFumigationProofOfTemperatureImages] TFPOTI
                            WHERE TFPOTI.FumigationRouteId = @FumigationRoutsId
                                  AND TFPOTI.IsApproved = 1
                                  AND TFPOTI.IsDeleted = 0;
                    END;
    END;