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
        public List<string> Items { get; set; }
        public string DocumentId { get; set; } = String.Empty;
        private readonly string collection = "meals";

        public TableViewSource(List<string> items,string day)
        {
            this.Items = items;
            this.SourceDay = day;
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
                    this.Save();
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
            DocumentReference doc;
            var mealDictionary = new NSMutableDictionary<NSString, NSObject>();
            var dishes = new NSMutableArray();
            var user = Auth.DefaultInstance.CurrentUser;
            for (int i = 0; i < this.Items.Count; i++)
            {
                dishes.Add(NSObject.FromObject(this.Items[i]));
                
            }
            NSString userNS = new NSString(MealDocument.UserKey);
            NSString dateNS = new NSString(MealDocument.DateKey);
            NSString meal = new NSString(this.Identifier);
            mealDictionary.SetValueForKey(new NSString(user.Uid), userNS);
            mealDictionary.SetValueForKey(NSObject.FromObject(this.SourceDay), dateNS);
            mealDictionary.SetValueForKey(dishes, meal);
            NSDictionary<NSString,NSObject> dictio =
                new NSDictionary<NSString, NSObject>(mealDictionary.Keys, mealDictionary.Values);
            doc = this.DocumentId == string.Empty
                ? Firestore.SharedInstance.GetCollection(collection).CreateDocument()
                : Firestore.SharedInstance.GetCollection(collection).GetDocument(this.DocumentId);
            if (this.DocumentId == string.Empty) this.DocumentId = doc.Id;
            doc.SetDataAsync(dictio);
       
        }
    }
}