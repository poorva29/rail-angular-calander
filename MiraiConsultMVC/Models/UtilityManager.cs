using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MiraiConsultMVC.Models;
using System.Web.Mvc;
namespace MiraiConsultMVC.Models
{
    public class UtilityManager
    {
        public static UtilityManager _instance = new UtilityManager();
        public UtilityManager() { }
        public static UtilityManager getInstance() { return _instance; }
        SqlConnection conn = null;

        public IEnumerable<SelectListItem> getAllCountries()
        {
            List<SelectListItem> dtCountry = new List<SelectListItem>();
            _dbAskMiraiDataContext _db = new _dbAskMiraiDataContext();

            var countryList = (from s in _db.countries
                               select new SelectListItem()
                               {
                                   Text = s.name,
                                   Value = s.countryid.ToString()
                               }).ToList();

            //foreach (var item in countryList)
            //{
            //    if (item != null)
            //    {
            //        SelectListItem selectItem = new SelectListItem();
            //        selectItem.Text = item.name.ToString();
            //        selectItem.Value = item.countryid.ToString();
            //        dtCountry.Add(selectItem);
            //    }
            //}

            return new SelectList(countryList);
        }   

        public DataTable getAllCountriescode()
        {
            DataTable dtCountrycode = null;
            
            return dtCountrycode;
        }
        public DataTable getAllCitiesByStateId(int stateid)
        {
            DataTable dtCity = null;
           
            return dtCity;
        }
        public DataTable getAllLocationByCityId(int cityid)
        {
            DataTable dtLocation = null;
          
            return dtLocation;
        }
        public DataTable getAllStatesByCountryId(int countryid)
        {
            DataTable dtState = null;
           
            return dtState;
        }
        //public int ActivateEmail(int userID, bool isEmailVerify, string email, out int isLinkActivate)
        //{
        //    int isl
        //    return 0;
        //}
        public DataSet getAllCities()
        {
            DataSet dsCities = null;

            return dsCities;
        }
        public DataSet getAllTags()
        {
            DataSet dsTags = null;

            return dsTags;
        }
        public DataTable getAllSpecialities()
        {
            DataTable dtSpecialities = null;
           
            return dtSpecialities;
        }
        public DataTable getAlltags()
        {
            DataTable dtTags = null;
           
            return dtTags;
        }
        public DataTable getAllRegistrationCouncilbyCountry(int countryId)
        {
            DataTable dtRegCouncil = null;
           
            return dtRegCouncil;
        }
        public DataSet getAllDegree()
        {
            DataSet dsdegree = null;
            
            return dsdegree;
        }
        public DataTable gelAllStatesbyCountry(int countryId)
        {
            DataSet dsStates = null;

           
            return null;
        }
        public DataSet getCountryStateCityLocation(int countryId, int stateId, int cityId, int locationId)
        {
            DataSet dsData = null;
            
            return null;
        }

        public int insertHomePageImages(string image, string homePageImage)
        {
            return 0;
        }

        public DataTable getHomePageImages()
        {
            DataTable dtImg = null;
            
            return dtImg;
        }
    }

    public class ItemCountries
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
    }

    public class CountryModel
    {
        private readonly List<ItemCountries> _Countries;
           

        public IEnumerable<SelectListItem> CountryItems
        {
            get { return new SelectList(_Countries, "CountryId", "Name"); }
        }
    }
}
