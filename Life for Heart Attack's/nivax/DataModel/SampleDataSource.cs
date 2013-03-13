using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Causes & Symptoms",
                    "Causes & Symptoms",
                    "Assets/Images/10.jpg",
                    "A heart attack occurs when blood flow to a part of your heart is blocked for a long enough time that part of the heart muscle is damaged or dies. The medical term for this is myocardial infarction.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Causes",
                    "Most heart attacks are caused by a blood clot that blocks one of the coronary arteries. The coronary arteries bring blood and oxygen to the heart. If the blood flow is blocked, the heart is starved of oxygen and heart cells die.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nA hard substance called plaque can build up in the walls of your coronary arteries. This plaque is made up of cholesterol and other cells.\n\nA heart attack may occur when:\nBlood platelets stick to tears in the plaque and form a blood clot that blocks blood from flowing to the heart. This is the most common cause of heart attacks.\n\nA slow buildup of this plaque may almost block one of your coronary arteries.\n\nThe cause of heart attacks is not always known. Heart attacks may occur:\nWhen you are resting or asleep\nAfter a sudden increase in physical activity\nWhen you are active outside in cold weather\nAfter sudden, severe emotional or physical stress,including an illness\nMany risk factors may lead to a heart attack.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Causes & Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Causes", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Life For Heart's" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Symptoms",
                     "A heart attack is a medical emergency. If you have symptoms of a heart attack, call 911 or your local emergency number right away.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nDO NOT try to drive yourself to the hospital.\nDO NOT WAIT. You are at greatest risk of sudden death in the early hours of a heart attack.\nChest pain is the most common symptom of a heart attack. You may feel the pain in only one part of your body, or it may move from your chest to your arms, shoulder, neck, teeth, jaw, belly area, or back.\nThe pain can be severe or mild. It can feel like:\nA tight band around the chest\nBad indigestion\nSomething heavy sitting on your chest\nSqueezing or heavy pressure\nThe pain usually lasts longer than 20 minutes. Rest and a medicine called nitroglycerin may not completely relieve the pain of a heart attack. Symptoms may also go away and come back.\nOther symptoms of a heart attack can include:\nAnxiety\nCough\nFainting\nLight-headedness\n dizziness\nNausea or vomiting\nPalpitations (feeling like your heart is beating too fast or irregularly)\nShortness of breath\nSweating, which may be very heavy\nSome people (the elderly, people with diabetes, and women) may have little or no chest pain. Or, they may have unusual symptoms (shortness of breath, fatigue, and weakness). A silent heart attack is a heart attack with no symptoms.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Causes & Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Symptoms", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Life For Heart's" });
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "Exams and Tests",
                   "Exams and Tests",
                   "Assets/Images/20.jpg",
                   "A doctor or nurse will perform a physical exam and listen to your chest using a stethoscope.The doctor may hear abnormal sounds in your lungs (called crackles), a heart murmur, or other abnormal sounds.You may have a fast or uneven pulse.");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Echocardiogram",
                    "An echocardiogram is a test that uses sound waves to create a moving picture of the heart. The picture is much more detailed than a plain x-ray image and involves no radiation exposure.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nHow the Test Is Performed\n\nTRANSTHORACIC ECHOCARDIOGRAM (TTE)\nTTE is the type of echocardiogram that most people will have.A trained sonographer performs the test, then a heart doctor interprets the results.\n\nAn instrument called a transducer that releases high-frequency sound waves is placed on your ribs near the breast bone and directed toward the heart. Other images will be taken underneath and slightly to the left of your nipple and in the upper abdomen.\nThe transducer picks up the echoes of sound waves and transmits them as electrical impulses. The echocardiography machine converts these impulses into moving pictures of the heart.Pictures can be two-dimensional or three-dimensional, depending on the part of the heart being evaluated and the type of machine.A Doppler echocardiogram uses a probe to record the motion of blood through the heart.\n\nAn echocardiogram allows doctors to see the heart beating, and to see the heart valves and other structures of the heart.Occasionally, your lungs, ribs, or body tissue may prevent the sound waves and echoes from providing a clear picture of heart function. If so, the sonographer may inject a small amount of liquid (contrast) through an IV to better see the inside of the heart.\n\nVery rarely, more invasive testing using special echocardiography probes may be needed.\n\nTRANSESOPHAGEAL ECHOCARDIOGRAM (TEE)\n\nThe back of your throat is numbed and a scope is inserted down your throat.\nOn the end of the scope is a device that sends out sound waves. An experienced technician will guide the scope down to the lower part of the esophagus. It is used to get a clearer echocardiogram of your heart.",
                    group2) { CreatedOn = "Group", CreatedTxt = "Exams and Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Echocardiogram", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/21.jpg")), CurrentStatus = "Life For Heart's" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "Exercise Stress Test",
                     "An exercise stress test is a screening tool used to test the effect of exercise on your heart.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow the Test Is Performed\n\nThis test is done at a medical center or health care provider's office.The technician will place 10 flat, sticky patches called electrodes on your chest. These are attached to an ECG monitor that follows the electrical activity of your heart during the test.\nYou will walk on a treadmill or pedal on an exercise bicycle. Slowly (usually every 3 minutes), you will be asked to walk (or pedal) faster and on an incline. It is like walking fast or jogging up a hill.While you exercise, the activity of your heart is measured with an electrocardiogram (ECG), and your blood pressure readings are taken.The test continues until:\nYou reach a target heart rate\nYou develop chest pain or a change in your blood pressure that is concerning\nECG changes show that your heart muscle is not getting enough oxygen\nYou are too tired or have other symptoms, such as leg pain, that keep you from continuing\nYou will be monitored for 10 - 15 minutes after exercising, or until your heart rate returns to baseline. The total time of the test is around 60 minutes.",
                     group2) { CreatedOn = "Group", CreatedTxt = "Exams and Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Exercise Stress Test", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/22.jpg")), CurrentStatus = "Life For Heart's" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Nuclear Stress Test",
                      "Thallium stress test is a nuclear imaging method that shows how well blood flows into the heart muscle, both at rest and during activity.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nHow the Test Is Performed\nThis test is done at a medical center or physician's office. It is done in parts, or stages:\nYou will have an IV (intravenous line) started.\nA radiopharmaceutical, such as thallium or sestamibi, will be injected into one of your veins.\nYou will lie down and wait for between 15 and 45 minutes.\nA special camera will scan your heart and create pictures to show how the radiopharmaceutical has traveled through your blood and into your heart.\nMost people will then walk on a treadmill (or pedal on an exercise machine).\nAfter the treadmill starts moving slowly, you will be asked to walk (or pedal) faster and on an incline. It is like being asked to walk fast or jog up a big hill.\nIf you are not able to exercise, your doctor may give you a medicine called a vasodilator, which dilates your heart arteries. Or you may get a medicine that will make your heart beat faster and harder, similar to when you exercise.\nYour blood pressure and heart rhythm (ECG) will be watched (monitored) the whole time.When your heart is working as hard as it can, a radiopharmaceutical is again injected into one of your veins.\nYou will wait for 15 to 45 minutes.\nAgain, the special camera will scan your heart and create pictures.\nYou may be allowed to get up from the table or chair and have a snack or drink.\n\nUsing a computer, your doctor can compare the first and second set of images. This can help your doctor tell if you have heart disease or if your heart disease is becoming worse.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams and Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Nuclear Stress Test", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/23.jpg")), CurrentStatus = "Life For Heart's" });
            this.AllGroups.Add(group2);
			
            var group3 = new SampleDataGroup("Group-3",
                   "Treatment",
                   "Treatment",
                   "Assets/Images/30.jpg",
                   "Treatment options for heart attack, and acute coronary syndrome, include:Oxygen therapy,Relieving pain and discomfort using nitroglycerin or morphine, Controlling any arrhythmias (abnormal heart rhythms) Blocking further clotting (if possible), using aspirin or clopidogrel (Plavix), as well as possibly anticoagulant drugs such as heparin");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "IMMEDIATE TREATMENTS TO SUPPORT THE PATIENT",
                    "Early supportive treatments are similar for patients who have ACS or those who have had a heart attack.Oxygen is almost always administered right away, usually through a tube that enters through the nose.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nEarly supportive treatments are similar for patients who have ACS or those who have had a heart attack.Oxygen is almost always administered right away, usually through a tube that enters through the nose.\nAspirin. The patient is given aspirin if one was not taken at home.\nMedications for Relieving Symptoms.\nNitroglycerin. Most patients will receive nitroglycerin during and after a heart attack, usually under the tongue. Nitroglycerin decreases blood pressure and opens the blood vessels around the heart, increasing blood flow. Nitroglycerin may be given intravenously in certain cases (recurrent angina, heart failure, or high blood pressure).\nMorphine. Morphine not only relieves pain and reduces anxiety but also opens blood vessels, aiding the circulation of blood and oxygen to the heart. Morphine can decrease blood pressure and slow down the heart. In patients in whom such effects may worsen their heart attacks, other drugs may be used.",
                    group3) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "IMMEDIATE TREATMENTS TO SUPPORT THE PATIENT", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/31.jpg")), CurrentStatus = "Life For Heart's" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                     "THROMBOLYTICS",
                     "Thrombolytic, also called clot-busting or fibrinolytic, drugs are recommended as alternatives to angioplasty. These drugs dissolve the clot, or thrombus, responsible for causing artery blockage and heart-muscle tissue death.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThrombolytic, also called clot-busting or fibrinolytic, drugs are recommended as alternatives to angioplasty. These drugs dissolve the clot, or thrombus, responsible for causing artery blockage and heart-muscle tissue death.\nGenerally speaking, thrombolysis is considered a good option for patients with full-thickness (STEMI) heart attacks when symptoms have been present for fewer than 3 hours. Ideally, these drugs should be given within 30 minutes of arriving at the hospital if angioplasty is not a viable option. Other situations where it may be used include when:\nProlonged transport will be required\nToo long of a time will pass before a catheterization lab is available\nPCI procedure is not successful or anatomically too difficult\nThrombolytics should be avoided or used with great caution in the following patients after heart attack:\nPatients older than 75 years\nWhen symptoms have continued beyond 12 hours\nPregnant women\nPeople who have experienced recent trauma (especially head injury) or invasive surgery\nPeople with active peptic ulcers\nPatients who have been given prolonged CPR\nCurrent users of anticoagulants\nPatients who have experienced any recent major bleeding\nPatients with low ST segments\nPatients with a history of stroke\nPatients with uncontrolled high blood pressure, especially when systolic is higher than 180 mm Hg\nSpecific Thrombolytics. The standard thrombolytic drugs are recombinant tissue plasminogen activators or rt-PAs.\n They include alteplase (Activase) and reteplase (Retavase) as well as a newer drug tenecteplase (TNKase).\nThrombolytic Administration. The sooner that thrombolytic drugs are given after a heart attack, the better. \nThe benefits of thrombolytics are highest within the first 3 hours. They can still help if given within 12 hours of a heart attack.\nComplications. Hemorrhagic stroke, usually occurring during the first day, is the most serious complication of thrombolytic therapy, but fortunately it is rare.",
                     group3) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "THROMBOLYTICS", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/32.jpg")), CurrentStatus = "Life For Heart's" });
            this.AllGroups.Add(group3);
        }
    }
}
