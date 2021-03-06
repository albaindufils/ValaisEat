﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DTO;
using DAL;
using Microsoft.Extensions.Configuration;

namespace BLL
{
    /*
    * Interface qui définit le comportement de la classe StaffsManager qui suit
    */
    public interface IStaffsManager
    {
        List<Staff> GetAll();
        Staff GetByID(int id);
        int Delete(int id);
        Staff Add(Staff staff);
        int Update(Staff staff);
        Staff GetByUsernamePassword(string username, string password);
        List<Staff> GetByCity(int id);
    }

    public class StaffsManager : IStaffsManager
    {
        private IStaffs_DB staff_db { get; }

        public StaffsManager(IStaffs_DB staff_db)
        {
            this.staff_db = staff_db;
;
        }

        public List<Staff> GetByCity(int id)
        {           
            return GetAll().Where(s => s.IdCity == id).ToList(); ;
        }
        public Staff GetByUsernamePassword(string username, string password)
        {
            return staff_db.GetByUsernamePassword(username, password);
        }
        public Staff Add(Staff staff)
        {
            return staff_db.Add(staff);
        }

        public int Delete(int id)
        {
            return staff_db.Delete(id);
        }

        public List<Staff> GetAll()
        {
            return staff_db.GetAll();
        }

        public Staff GetByID(int id)
        {
            return staff_db.GetByID(id);
        }

        public int Update(Staff staff)
        {
            return staff_db.Update(staff);
        }
    }
}
