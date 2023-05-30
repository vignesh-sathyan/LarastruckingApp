using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Enum
{
    public enum QuoteStatus
    {

        APPROVED=1,
        SUBMITTED=2,
        [Display(Name = "ON HOLD")]
        ONHOLD =3,
        DECLINED=4,
               
    }

    public enum LeaveStatus
    {

        Pending = 1,
        Approved = 2,
        Unapproved = 3,
        Cancelled = 4
    }

    public enum Unit
    {
        KG = 1,
        LBS = 2
    }

    public enum PricingMethod
    {
        PLTS = 1,
        BXS = 2,
        Trailer = 3
    }

    public enum SpType
    {
        Insert = 1,
        Update = 2
    }

    #region Equipment Colors
    /// <summary>
    /// Use this enum in equipment to bind color dropment
    /// </summary>
    public enum Colors
    {
        // Don't change the numbering
        Black = 1,
        Green = 2,
        Grey = 3,
        Red = 4,
        Silver = 5,
        White = 6
    }
    #endregion

    #region Equipment Roller Bed
    /// <summary>
    /// Use this enum in equipment to bind roller bed radio
    /// </summary>
    public enum RollerBed
    {
        // Don't change the numbering
        Roller = 1,
        Bed = 2
    }
    #endregion

    #region Equipment DoorType
    /// <summary>
    /// Use this enum in equipment to bind door type
    /// </summary>
    public enum DoorType
    {
        // Don't change the numbering
        [Display(Name = "Lift Door")]
        LiftDoor = 1,
        [Display(Name = "Roller Door")]
        RollerDoor = 2,
        [Display(Name = "Swing Door")]
        SwingDoor = 3
    }
    #endregion

    #region Equipment Owned By
    /// <summary>
    /// Use this enum in equipment to owned by dropdown
    /// </summary>
    public enum OwnedBy
    {
        // Don't change the numbering
        Laras = 1,
        Leasing = 2
    }
    #endregion

    #region Pre-Trip Image Type
    /// <summary>
    /// Use this enum in the pre-trip shipment detail when uploading proof of temp and damage product pictures
    /// </summary>
    public enum PreTripImageType : int
    {
        // Don't change the numbering
        Proof = 1,
        Damage = 2
    }
    #endregion

}
