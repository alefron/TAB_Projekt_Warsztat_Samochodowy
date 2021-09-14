using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormAddEditPersonel
    {

        PersonelRepository personelRepository = new PersonelRepository();

        public Personel personel { get; set; }

        public bool isAdmin { get; set; }

        // constructor for Add
        public FormAddEditPersonel()
        {
            this.isAdmin = true;
            this.personel = new Personel();
            this.personel.Address = new Address();
            this.personel.Role = new Role();
        }

        public FormAddEditPersonel(int personelId, bool isAdmin)
        {
            this.isAdmin = isAdmin;
            this.personel = personelRepository.GetJoinedPersonelById(personelId);
        }

    }
}
