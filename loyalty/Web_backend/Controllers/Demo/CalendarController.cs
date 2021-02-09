using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.Common;


namespace Palmary.Loyalty.Web_backend.Controllers.Demo
{
    [Authorize]
    public class CalendarController : Controller
    {
        //step2: for data grid header
        public string instructor()
        {
            var list = new List<InstructorObject>();

            list.Add(new InstructorObject()
            {
                instructorId = "1",
                instructorName = "Peter Chan"
            });

            list.Add(new InstructorObject()
            {
                instructorId = "2",
                instructorName = "Ray Li"
            });

            return new { data = list }.ToJson();
        }

        public string daily(string instructorId)
        {
            var list = new List<InstructorObject>();
            var ilist = new List<InformationObject>();

            if (instructorId == " " || instructorId == "")
            {
                list.Add(new InstructorObject()
                {
                    instructorId = "1",
                    instructorName = "Peter Chan"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "2",
                    instructorName = "Ray Li"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "3",
                    instructorName = "Mary Lee"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "4",
                    instructorName = "Simon Wong"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "5",
                    instructorName = "Paul W",
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "6",
                    instructorName = "Winine M"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "7",
                    instructorName = "TK Wong"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "8",
                    instructorName = "Viann Ma"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "9",
                    instructorName = "Paul Lau"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "10",
                    instructorName = "Pang Chau"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "11",
                    instructorName = "Ming Lai"
                });

                ilist.Add(new InformationObject()
                {
                    instructorId = "1",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE"
                });

                ilist.Add(new InformationObject()
                {
                    instructorId = "2",
                    content = "Annual Leave",
                    week_day = "7",
                    time_length = "720",
                    start_time = "09:00",

                    color = "#CCCCFF"

                });

                ilist.Add(new InformationObject()
                {
                    instructorId = "2",
                    content = "Test",
                    week_day = "7",
                    time_length = "60",
                    start_time = "13:00",
                    color = "#A4D3EE"

                });

                ilist.Add(new InformationObject()
                {
                    instructorId = "3",
                    content = "Training<br>Pony<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "11:00",
                    color = "#A4D3EE"

                });



                ilist.Add(new InformationObject()
                {
                    instructorId = "1",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE"

                });
                ilist.Add(new InformationObject()
                {
                    instructorId = "11",
                    content = "Annual Leave",
                    week_day = "7",
                    time_length = "720",
                    start_time = "09:00",
                    color = "#FAEBD7"
                });


                ilist.Add(new InformationObject()
                {
                    instructorId = "3",
                    content = "Training<br>Horse22<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "13:00",
                    color = "#A4D3EE"

                });
            }
            else
            {
                list.Add(new InstructorObject()
                {
                    instructorId = "1",
                    instructorName = "instructor Name1"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "2",
                    instructorName = "instructor Name2"
                });

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "3",
                //    instructorName = "instructor Name3"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "4",
                //    instructorName = "instructor Name4"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "5",
                //    instructorName = "instructor Name5"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "6",
                //    instructorName = "instructor Name6"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "7",
                //    instructorName = "instructor Name7"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "8",
                //    instructorName = "instructor Name8"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "9",
                //    instructorName = "instructor Name9"
                //});

                //list.Add(new InstructorObject()
                //{
                //    instructorId = "10",
                //    instructorName = "instructor Name10"
                //});

                ilist.Add(new InformationObject()
                {
                    instructorId = "1",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE"

                });

                ilist.Add(new InformationObject()
                {
                    instructorId = "2",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "14:00",
                    color = "#A4D3EE"

                });


                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "3",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "4",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "5",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "6",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "7",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "8",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "9",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});

                //ilist.Add(new InformationObject()
                //{
                //    instructorId = "10",
                //    content = "Learning to Ride<br>small paddock<br>(1/6)",
                //    week_day = "1",
                //    time_length = "120",
                //    start_time = "14:00",
                //    color = "#A4D3EE"

                //});
            }

            return new { instructors = list, information = ilist }.ToJson();
        }

        public string weekly(string instructorId)
        {
            var list = new List<InstructorObject>();
            var ilist = new List<InformationObject>();
            var wlist = new List<WeekdayObject>();

            wlist.Add(new WeekdayObject()
            {
                day = "2014-07-26<br>(Sat)",
                colorStyle = "color:white;background:green;"
            });

            wlist.Add(new WeekdayObject()
            {
                day = "2014-07-27<br>(Sun)",
                colorStyle = "color:blue;background:yellow;"
            });

            wlist.Add(new WeekdayObject()
            {
                day = "2014-07-28<br>(Mon)",
                colorStyle = ""
            });

            wlist.Add(new WeekdayObject()
            {
                day = "2014-07-29<br>(Tue)",
                colorStyle = "color:blue;background:red;"
            });

            wlist.Add(new WeekdayObject()
            {
                day = "2014-07-30<br>(Wed)",
                colorStyle = "color:blue;background:yellow;"
            });

            wlist.Add(new WeekdayObject()
            {
                day = "2014-07-31<br>(Thu)",
                colorStyle = "color:blue;background:yellow;"
            });

            wlist.Add(new WeekdayObject()
            {
                day = "2014-08-01<br>(Fri)",
                colorStyle = "color:blue;background:yellow;"
            });

            if (instructorId == " " || instructorId == "")
            {

                list.Add(new InstructorObject()
                {
                    instructorId = "1",
                    instructorName = "Peter Chan"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "2",
                    instructorName = "Ray Li"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "3",
                    instructorName = "Mary Lee"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "4",
                    instructorName = "Simon Wong"
                });

                list.Add(new InstructorObject()
                {
                    instructorId = "5",
                    instructorName = "Paul W",
                });

                ilist.Add(new InformationObject()
                {
                    instructorId = "1",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE",
                    button_text = "Test1",
                    button_tag_id = "tag_t1",
                    button_tag_name = "Tag Test 1",
                    button_url = "com.palmary.user.js.edit",
                    button_target_id = "1"

                });
                ilist.Add(new InformationObject()
                {
                    instructorId = "2",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE",
                    button_text = "Test2",
                    button_tag_id = "tag_t2",
                    button_tag_name = "Tag Test 2",
                    button_url = "com.palmary.user.js.edit",
                    button_target_id = "1360"

                });
                ilist.Add(new InformationObject()
                {
                    instructorId = "3",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE",
                });
            }
            else
            {
                list.Add(new InstructorObject()
                {
                    instructorId = "1",
                    instructorName = "Peter Chan"
                });


                ilist.Add(new InformationObject()
                {
                    instructorId = "1",
                    content = "Learning to Ride<br>small paddock<br>(1/6)",
                    week_day = "1",
                    time_length = "120",
                    start_time = "12:00",
                    color = "#A4D3EE",
                    button_text = "TestA",
                    button_tag_id = "tag_tA",
                    button_tag_name = "Tag Test A",
                    button_url = "com.palmary.user.js.edit",
                    button_target_id = "1"

                });

            }
            return new { success = true, weekDays = wlist, instructors = list, information = ilist }.ToJson();
        }
    }

    public class InstructorObject
    {
        public string instructorId;
        public string instructorName;
    }

    public class InformationObject
    {
        public string instructorId;
        public string content;
        public string week_day;
        public string time_length;
        public string start_time;
        public string color;
        public string button_text;
        public string button_url;
        public string button_tag_id;
        public string button_tag_name;
        public string button_target_id;
    }

    public class WeekdayObject
    {
        public string day;
        public string colorStyle;
    }
}