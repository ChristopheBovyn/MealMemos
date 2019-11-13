using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.CloudFirestore;
using Foundation;
using MealMemos.Models;
using UIKit;


namespace MealMemos.iOS
{
    public class TableViewSource : UITableViewSource
    {
        public string Identifier = string.Empty;
        public string SourceDay;
        private string documentId = string.Empty;
        private const string collection = "meals";
        List<string> Items;
        public TableViewSource(List<string> items, string day)
        {
            this.Items = items;
            this.SourceDay = day;
        }

        public void SetDocumentId(string id)
        {
            this.documentId = id;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("customCell", indexPath);
            string item = Items[indexPath.Row];
            cell.TextLabel.Text = item;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Items.Count;
        }

        public void AddElement(string element)
        {
            this.Items.Add(element);
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // remove the item from the underlying data source
                    this.Items.RemoveAt(indexPath.Row);
                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.None);
                    //this.Save();
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true; // return false if you wish to disable editing for a specific indexPath or for all rows
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {   // Optional - default text is 'Delete'
            return "Delete";
        }

        public void Save()
        {
            ////var dictionary = new Dictionary<string, object>();
            //var user = Auth.DefaultInstance.CurrentUser;
            //var dishes = new List<string>();
            //for (int i = 0; i < this.Items.Count; i++)
            //{
            //    dishes.Add(this.Items[i]);
            //}
            ////dictionary.Add(MealDocument.UserKey, user.Uid);
            ////dictionary.Add(MealDocument.DateKey, this.SourceDay);
            ////dictionary.Add(this.Identifier, dishes);
            //DocumentReference doc;
            //var mealDictionary = new NSDictionary<NSString, NSObject>();
            //mealDictionary.SetValueForKey(user, (NSString)MealDocument.UserKey);
            //mealDictionary.SetValueForKey(NSObject.FromObject(this.SourceDay), (NSString)MealDocument.DateKey);
            //mealDictionary.SetValueForKey(NSObject.FromObject(dishes), (NSString)this.Identifier);
            //doc = this.documentId == string.Empty ? Firestore.SharedInstance.GetCollection(collection).CreateDocument()
            //    : Firestore.SharedInstance.GetCollection(collection).GetDocument(this.documentId);
            //if (this.documentId == string.Empty) this.documentId = doc.Id;
            //doc.SetDataAsync(mealDictionary);
        }
    }
}