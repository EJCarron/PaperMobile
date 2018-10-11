using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;



namespace The_Model {
    public delegate void Watcher(EModelUpdateType updateType);

    public class ModelItem {
        public string dataKey;
        public object dataItem;
        public bool removed = false;

        public List<Watcher> watchers = new List<Watcher>();


        public ModelItem(string dataKey, object dataItem) {

            this.dataKey = dataKey;
            this.dataItem = dataItem;
        }

        public void addWatcher(Watcher watcher) {

            this.watchers.Add(watcher);

        }



        public void updateModelItem(DataItemUpdate update) {

            if (update.remove) {
                this.removed = true;
            } else {
                this.dataItem = update.dataItem;
            }



        }

    }

    public class DataItemUpdate {
        public string dataKey;
        public object dataItem;
        public bool remove = false;

        public DataItemUpdate(string dataKey, object dataItem) {

            this.dataKey = dataKey;
            this.dataItem = dataItem;
        }


    }




    public class DataItemUpdate_Remove : DataItemUpdate {



        public DataItemUpdate_Remove(string dataKey) : base(dataKey, null) {

            this.remove = true;
        }
    }



    public enum EModelUpdateType {
        removal = 0,
        dataUpdate = 1
    }



    public class ModelStore : IModelStore {
        private Dictionary<string, ModelItem> modelItems = new Dictionary<string, ModelItem>();

        public void updateItems(params DataItemUpdate[] updates) {

            this.updateItems((IList<DataItemUpdate>)updates);
        }

        public void updateItems(IList<DataItemUpdate> listOfUpdates, params DataItemUpdate[] updatesParam) {//}) {

            foreach (DataItemUpdate update in updatesParam) {
                listOfUpdates.Add(update);
            }


            Collection<ModelItem> removedModelItems = new Collection<ModelItem>();

            List<Watcher> watchersToAlert = new List<Watcher>();
            List<Watcher> watchersToAlertAboutRemoval = new List<Watcher>();

            foreach (DataItemUpdate update in listOfUpdates) {

                ModelItem modelItemToBeUpdated = null;

                if (update.remove) {


                    if (this.modelItems.ContainsKey(update.dataKey)) {

                        modelItemToBeUpdated = this.modelItems[update.dataKey];

                        removedModelItems.Add(modelItemToBeUpdated);

                        this.modelItems.Remove(update.dataKey); // remove from list of model items

                        foreach (Watcher watcher in modelItemToBeUpdated.watchers) {

                            if (!watchersToAlertAboutRemoval.Contains(watcher)) {

                                watchersToAlertAboutRemoval.Add(watcher);
                            }
                        }
                    }

                } else {

                    if (!this.modelItems.ContainsKey(update.dataKey)) {

                        ModelItem new_modelItem = new ModelItem(update.dataKey, update.dataKey);

                        this.modelItems.Add(update.dataKey, new_modelItem);

                    }
                    modelItemToBeUpdated = this.modelItems[update.dataKey];
                }
                if (modelItemToBeUpdated != null) {
                    modelItemToBeUpdated.updateModelItem(update);
                }

            }

            // get list of watchers wathcing NON removal updates.

            foreach (DataItemUpdate update in listOfUpdates) {
                if (!update.remove) {
                    foreach (Watcher watcher in this.modelItems[update.dataKey].watchers) {

                        if (!watchersToAlert.Contains(watcher)) {

                            watchersToAlert.Add(watcher);
                        }
                    }
                }
            }

            // allert watchers of removals

            foreach (Watcher watcher in watchersToAlertAboutRemoval) {
                watcher(EModelUpdateType.removal);
            }

            // allert watchers of changes;

            foreach (Watcher watcher in watchersToAlert) {

                if (areAllWatcherTargetsLoaded(watcher)) {

                    watcher(EModelUpdateType.dataUpdate);
                }
            }

        }


        public bool areAllWatcherTargetsLoaded(Watcher watcher) {
            List<ModelItem> targets = new List<ModelItem>();


            foreach (KeyValuePair<string, ModelItem> modelItem in this.modelItems) {

                if (modelItem.Value.watchers.Contains(watcher)) {

                    targets.Add(modelItem.Value);
                }
            }

            foreach (ModelItem target in targets) {

                if (target.dataItem == null) {

                    return false;
                }
            }

            return true;
        }




        public void addWatcher(Watcher watcher, params string[] dataKeys) {

            foreach (string dataKey in dataKeys) {

                if (!this.modelItems.ContainsKey(dataKey)) {

                    ModelItem new_modelItem = new ModelItem(dataKey, null);

                    this.modelItems.Add(dataKey, new_modelItem);

                    new_modelItem.addWatcher(watcher);
                } else {
                    this.modelItems[dataKey].addWatcher(watcher);

                    if (areAllWatcherTargetsLoaded(watcher)) {

                        watcher(EModelUpdateType.dataUpdate);
                    }

                }
            }



        }

        public void RemoveWatcher() {



        }

        public T getItem<T>(string dataKey) {

            if (!this.modelItems.ContainsKey(dataKey) || (this.modelItems[dataKey].dataItem == null)) {

                return default(T);

            } else {
                return (T)this.modelItems[dataKey].dataItem;

            }

        }
    }
}
