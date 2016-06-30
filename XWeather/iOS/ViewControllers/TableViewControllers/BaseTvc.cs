﻿using System;

using Foundation;
using UIKit;

using XWeather.Clients;
using System.Linq;

namespace XWeather.iOS
{
	public class BaseTvc<TCell> : UITableViewController
		where TCell : BaseTvCell
	{

		nfloat headerHeight = headerBase;

		static nfloat headerBase = 280;

		static nfloat footerBase = 44;


		public WuLocation Location => WuClient.Shared.Current;


		public BaseTvc (IntPtr handle) : base (handle) { }


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.Clear;

			TableView.BackgroundColor = UIColor.Clear;
		}


		[Export ("scrollViewDidScroll:")]
		public void Scrolled (UIScrollView scrollView)
		{
			MaskCells (scrollView);
		}


		public void MaskCells (UIScrollView scrollView)
		{
			foreach (TCell cell in TableView.VisibleCells) {

				var topHiddenHeight = scrollView.ContentOffset.Y + headerHeight - cell.Frame.Y + scrollView.ContentInset.Top;
				var bottomHiddenHeight = cell.Frame.Bottom - (scrollView.ContentOffset.Y + scrollView.Frame.Height - footerBase);

				cell.SetCellMask (topHiddenHeight, bottomHiddenHeight);
			}
		}


		public override nfloat GetHeightForHeader (UITableView tableView, nint section) => headerHeight;


		public override nfloat GetHeightForFooter (UITableView tableView, nint section) => footerBase;


		public override void WillDisplayHeaderView (UITableView tableView, UIView headerView, nint section)
		{
			if (TableView.VisibleCells.Any ()) MaskCells (TableView);

			var header = headerView as UITableViewHeaderFooterView;

			if (header?.ContentView != null)
				header.ContentView.BackgroundColor = UIColor.Clear;

			if (header?.BackgroundView != null)
				header.BackgroundView.BackgroundColor = UIColor.Clear;

			if (header?.TextLabel != null)
				header.TextLabel.TextColor = UIColor.White;
		}


		public override void WillDisplayFooterView (UITableView tableView, UIView footerView, nint section)
		{
			var footer = footerView as UITableViewHeaderFooterView;

			if (footer?.ContentView != null)
				footer.ContentView.BackgroundColor = UIColor.Clear;

			if (footer?.BackgroundView != null)
				footer.BackgroundView.BackgroundColor = UIColor.Clear;

			if (footer?.TextLabel != null)
				footer.TextLabel.TextColor = UIColor.White;
		}


		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;


		public TCell DequeCell (UITableView tableView, NSIndexPath indexPath)
			=> tableView.DequeueReusableCell (typeof (TCell).Name, indexPath) as TCell;
	}
}