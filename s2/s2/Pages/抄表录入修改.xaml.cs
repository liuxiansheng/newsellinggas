﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using Com.Aote.ObjectTools;
using Com.Aote.Behaviors;

namespace Com.Aote.Pages
{
    public partial class 抄表录入修改 : UserControl
    {
        int id = 0;
        public 抄表录入修改()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //拿出datagrid所选数据
            GeneralObject go = handUserUnits.SelectedItem as GeneralObject;
            //拿出页面数据上下文
            GeneralObject updatehandplan = handUserUnit.DataContext as GeneralObject;
            //新建对象，往t_updatehandplan插入数据
            GeneralObject obj = new GeneralObject();
            try
            {
                obj.WebClientInfo = Application.Current.Resources["dbclient"] as WebClientInfo;
                obj.EntityType = "t_updatehandplan";
                obj.SetPropertyValue("f_userid", ui_userid.Text, false);
                obj.SetPropertyValue("f_username", ui_username.Text, false);
                obj.SetPropertyValue("f_address", ui_address.Text, false);
                obj.SetPropertyValue("oughtfee", decimal.Parse(go.GetPropertyValue("oughtfee").ToString()), false);
                obj.SetPropertyValue("newoughtfee", decimal.Parse(ui_oughtfee.Text), false);
                if (go.GetPropertyValue("lastinputgasnum") != null) {
                    obj.SetPropertyValue("lastinputgasnum", decimal.Parse(go.GetPropertyValue("lastinputgasnum").ToString()), false);
                }
                //修改后上期指数
                if (updatehandplan.GetPropertyValue("lastinputgasnum") != null)
                {
                    obj.SetPropertyValue("newlastinputgasnum", decimal.Parse(updatehandplan.GetPropertyValue("lastinputgasnum").ToString()), false);
                }

                obj.SetPropertyValue("lastrecord", decimal.Parse(go.GetPropertyValue("lastrecord").ToString()), false);
                obj.SetPropertyValue("newlastrecord", decimal.Parse(ui_lastrecord.Text), false);
                obj.SetPropertyValue("shifoujiaofei", go.GetPropertyValue("shifoujiaofei").ToString(), false);
                obj.SetPropertyValue("f_newzhinajindate", ui_zhinajindate.SelectedDate.Value, false);
                //obj.SetPropertyValue("f_zhinajindate", go.GetPropertyValue("f_zhinajindate").ToString(), false);
                obj.SetPropertyValue("newshifoujiaofei", ui_shifoujiaofei.Text, false);
                obj.SetPropertyValue("f_updatenote", ui_updatenote.Text, false);
                obj.SetPropertyValue("lastinputdate", go.GetPropertyValue("lastinputdate"), false);
                obj.SetPropertyValue("newlastinputdate", ui_lastinputdate.SelectedDate, false);
                if (go.GetPropertyValue("f_inputtor") == null)
                {
                    go.SetPropertyValue("f_inputtor", "无", false);
                }
                obj.SetPropertyValue("f_inputtor", go.GetPropertyValue("f_inputtor").ToString(), false);
                obj.SetPropertyValue("f_newinputtor", ui_inputtor.Text, false);
                obj.SetPropertyValue("oughtamount", decimal.Parse(go.GetPropertyValue("oughtamount").ToString()), false);
                obj.SetPropertyValue("newoughtamount", decimal.Parse(ui_oughtamount.Text), false);
                obj.SetPropertyValue("f_handplanoperator", ui_handplanoperator.Text, false);
                obj.SetPropertyValue("f_handplandate", ui_handplandate.SelectedDate, false);
                obj.Name = "t_updatehandplan";
                //obj.Completed += obj_Completed;
                obj.Save();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
          
            //oughtfee shifoujiaofei f_operator f_inputtor f_zhinajindate
            //拼接更新sql

            string sql = "update t_handplan set lastrecord= " + decimal.Parse(ui_lastrecord.Text) +
                ",oughtfee=" + decimal.Parse(ui_oughtfee.Text) +
                ",shifoujiaofei='" + ui_shifoujiaofei.Text +
                "',f_operator='" + ui_operator.Text +
                "',f_inputtor='" + ui_inputtor.Text +
                "',f_zhinajindate='" + ui_zhinajindate.SelectedDate.ToString().Substring(0, 10) +

                "',oughtamount=" + decimal.Parse(ui_oughtamount.Text);
            if (updatehandplan.GetPropertyValue("lastinputgasnum") != null)
            {
                sql += ",lastinputgasnum=" + updatehandplan.GetPropertyValue("lastinputgasnum").ToString();
            }
            sql+="  where id = " + go.GetPropertyValue("id");
            HQLAction action = new HQLAction();
            action.HQL = sql;
            action.WebClientInfo = Application.Current.Resources["dbclient"] as WebClientInfo;
            action.Name = "abc";
            action.Invoke();
            //如果数据有误，页面提示
            //回调页面保存按钮功能
            //BatchExcuteAction save = (from p in loader.Res where p.Name.Equals("SaveAction") select p).First() as BatchExcuteAction;
            // save.Invoke();
            PagedObjectList save1 = (from p in loader.Res where p.Name.Equals("personlist") select p).First() as PagedObjectList;
            save1.IsOld = true;
            updatehandplan.New();
        }

    }
}