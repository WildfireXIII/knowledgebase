﻿using System;
//using System.IO;
using System.Web;
using Android.App;
using Android.Content.Res;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using DWL.Utility;
using System.IO;

namespace App
{
	[Activity(Label = "App", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
	public class MainActivity : Activity
	{
		int count = 1;

		WebView m_pWebView;

		private string[] m_aNavTitles;
		private DrawerLayout m_pDrawerLayout;
		private ListView m_pDrawerList;

		private string m_sCSS;
		private string m_sHead;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			WebCommunications.AuthKey = "54003c32a190b6063fe06a528bc230ce151b589512db4a39ecf8ac01be393dafa154f39bde1f56e690f4c3c2870323972240d4d02fc4fa2f3349dc7ef4c7dc09";

			// load important stuff
			this.LoadCSS();

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			//Button button = FindViewById<Button>(Resource.Id.MyButton);

			//button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

			string[] m_aNavTitles = new string[] { "thing1", "thing2" };

			m_pDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.appDrawerLayout);
			m_pDrawerList = FindViewById<ListView>(Resource.Id.appDrawerList);

			m_pDrawerList.Adapter = new DrawerItemCustomAdapter(this, Resource.Layout.ListViewItemRow, m_aNavTitles);

			m_pWebView = FindViewById<WebView>(Resource.Id.webview);
			m_pWebView.SetWebViewClient(new CustomWebViewClient());
			m_pWebView.Settings.JavaScriptEnabled = true;
			//m_pWebView.LoadUrl("http://www.google.com");
			//m_pWebView.Load
			Query("Genetic_Algorithm");
		}

		private void Query(string sQuery)
		{
			string sFixedQuery = HttpUtility.UrlEncode(sQuery);
			string sResponse = WebCommunications.SendGetRequest("http://dwlapi.azurewebsites.net/api/reflection/KnowledgeBaseServer/KnowledgeBaseServer/KnowledgeServer/ConstructPage?squery=" + sFixedQuery, true);

			// fix response and add to it
			sResponse = Master.CleanResponse(sResponse);

			string sHTML = "<html><head><style>" + m_sCSS + "</style></head>" + sResponse;
			
			m_pWebView.LoadData(sHTML, "text/html", "UTF-8");
		}

		private void LoadCSS()
		{

			AssetManager pManager = this.Assets;
		
			var pStream = pManager.Open("Style.css");
			StreamReader pStreamReader = new StreamReader(pStream);
			m_sCSS = pStreamReader.ReadToEnd();
		}

		private void LoadHeaderHtml()
		{
		
		}
	}
}

