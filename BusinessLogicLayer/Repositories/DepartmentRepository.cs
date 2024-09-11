using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class DepartmentRepository : GenericRepositiry<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DataContext context) : base(context)
        {
        }

    }
}
