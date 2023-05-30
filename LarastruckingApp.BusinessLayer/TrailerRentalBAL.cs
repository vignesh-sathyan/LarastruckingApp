using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.TrailerRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class TrailerRentalBAL : ITrailerRentalBAL
    {
        private readonly ITrailerRentalDAL trailerRentalDAL = null;
        public TrailerRentalBAL(ITrailerRentalDAL itrailerRentalDAL)
        {
            trailerRentalDAL = itrailerRentalDAL;
        }
               
        #region Save Trailer Rental
        /// <summary>
        /// Save trailer rental detial
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveTrailerRental(TrailerRentalDTO model)
        {
            return trailerRentalDAL.SaveTrailerRental(model);
        }
        #endregion
        #region get trailer rental list
        /// <summary>
        /// get trailer rental list
        /// </summary>
        /// <returns></returns>
        public IList<TrailerRentalListDTO> GetTrailerRentalList(DataTableFilterDto entity)
        {
            return trailerRentalDAL.GetTrailerRentalList(entity);
        }


        #endregion

        #region Get Trailer Rental Detail by id
        /// <summary>
        /// get trailer rental detial by id
        /// </summary>
        /// <param name="trailerRentalId"></param>
        /// <returns></returns>
        public TrailerRentalDTO GetTrailerRentalDetailById(int trailerRentalId)
        {
            return trailerRentalDAL.GetTrailerRentalDetailById(trailerRentalId);
        }


        #endregion

        #region Edit Trailer Rental
        /// <summary>
        /// Edit trailer rental detial
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditTrailerRental(TrailerRentalDTO model)
        {
            return trailerRentalDAL.EditTrailerRental(model);
        }

        #endregion

        #region delete trailer rental
        /// <summary>
        /// Delete trailer rental
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteTrailerRental(TrailerRentalDTO model)
        {
            return trailerRentalDAL.DeleteTrailerRental(model);
        }
        #endregion
    }
}
