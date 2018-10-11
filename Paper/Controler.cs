using System;
using The_Model;

namespace Paper {
    public class Controler {
        private ModelStore model;

        public Controler(ModelStore model) {
            this.model = model;
        }

        //public void increaseWidth(){

        //    model.updateItems(new DataItemUpdate(DataKeys.width,(this.model.getItem<double>(DataKeys.width) + 10)));
        //}

        //public void increaseHeight(){
            
        //    model.updateItems(new DataItemUpdate(DataKeys.height, (this.model.getItem<double>(DataKeys.height)+10) ));
        //}
    }
}
