using System;
using System.Collections.Generic;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormAddActionType
    {
        public int proposalId { get; set; }
        public int actionId { get; set; }
        public bool isEdit { get; set; }

        public List<ActionType> actions { get; set; }

        ActionTypeRepository actionTypeRepository = new ActionTypeRepository();

        public FormAddActionType(int proposalId, int actionId, bool isEdit)
        {
            this.proposalId = proposalId;
            this.actionId = actionId;
            this.isEdit = isEdit;
            this.actions = this.actionTypeRepository.GetList();
        }
    }
}
