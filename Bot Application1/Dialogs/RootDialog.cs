using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    /*public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }*/
    [LuisModel("af16dd76-33bb-48bb-b460-c1d48f5ee944", "1b4742c04aac403094f33c5275a33eec")]
    public class RootDialog : LuisDialog<object>
    {
        private DateTime msgReceivedDate;
        public RootDialog()
        {
            msgReceivedDate = DateTime.Now;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("GreetWelcome")]
        public async Task GreetWelcome(IDialogContext context, LuisResult luisResult)
        {
            string response = string.Empty;
            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response = $"Good morning. How can I be of any assistantance to you? :)";
            }
            else
            {
                response = $"Hello there. How can I be of any assistantance to you? :)";
            }
            await context.PostAsync(response);
            //await context.PostAsync("Hello there :). How can I be of any assistantance to you?");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("GreetFarewell")]
        public async Task GreetFarewell(IDialogContext context, LuisResult luisResult)
        {
            string response = string.Empty;
            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response = $"Good bye, have a nice day. :)";
            }
            else
            {
                response = $"B'bye, take care :)";
            }
            await context.PostAsync(response);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Admission")]
        public async Task Admission(IDialogContext context, LuisResult luisResult)
        {

            await context.PostAsync("Admission forms will be available after 1 January. Be sure to check our admission page http://www.uok.edu.pk/admissions/");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Faculties")]
        public async Task Faculties(IDialogContext context, LuisResult luisResult)
        {
            await context.PostAsync("Karachi University have many faculties for students to choose from. Faculty of Education, Engineering, Islamic Studies, Law, Management & Administrative Science, Medicine, Pharmacy, Science and Social Sciences.");
            await context.PostAsync("If you want more details of any specific faculty and its department just ask. :)");
            //await context.PostAsync("Department in which you can enroll.\n*Education\n*Teacher Education\n*Special Education\n*Chemical Engineering\n*Islam Learning\n*School of Law\n*KU Business School\n*Public Administration\n*Public Administration\n*Commerce\n*Pharmaceutical Chemistry\n*Computer Science");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Faculty")]
        public async Task Faculty(IDialogContext context, LuisResult luisResult)
        {
            EntityRecommendation facultyName;
            string name = string.Empty;
            if (luisResult.TryFindEntity("Faculty", out facultyName))
            {
                name = facultyName.Entity;
            }
            switch (name)
            {
                case "education":
                    await context.PostAsync("The Faculty of Education is comprised of the following Departments:");
                    await context.PostAsync("*Education,\n*Special Education,\n*Teacher Education.");
                    break;
                case "engineering":
                    await context.PostAsync("The Faculty of Engineering is comprised of the following Departments:");
                    await context.PostAsync("*Chemical Engineering.");
                    break;
                case "islam":
                case "islam studies":
                case "islamic studies":
                    await context.PostAsync("The Faculty of Islamic Studies is comprised of the following Departments:");
                    await context.PostAsync("*Islam Learning,\n*Quran Wa Sunnah,\n*Usool-ud-Din,\n*Sirah Chair.");
                    break;
                case "law":
                    await context.PostAsync("The Faculty of Law is comprised of the following Departments:");
                    await context.PostAsync("*School of Law.");
                    break;
                case "management":
                case "management science":
                case "administrative":
                case "administrative science":
                case "management and administrative":
                case "management & administrative":
                case "management and administrative science":
                case "management & administrative science":
                    await context.PostAsync("The Faculty of Management & Administrative Science is comprised of the following Departments:");
                    await context.PostAsync("*Karachi University Business School,\n*Public Administration,\n*Commerce.");
                    break;
                case "pharmacy":
                    await context.PostAsync("The Faculty of Pharmacy is comprised of the following Departments:");
                    await context.PostAsync("*Pharmaceutical Chemistry,\n*Pharmaceutics,\n*Pharmacology,\n*Pharmacognosy.");
                    break;
                case "science":
                    await context.PostAsync("The Faculty of Science is comprised of the following Departments:");
                    await context.PostAsync("*Agriculture and Agribusiness Management,\n*Applied Chemistry & Chemical Technology,\n*Applied Physics,\n*Biochemistry,\n*Biotechnology,\n*Botany,\n*Chemistry,\n*Computer Science,\n*Food Science & Technology,\n*Genetics,\n*Geography,\n*Geology,\n*Health Physical Education & Sports Sciences,\n*Mathematics,\n*Microbiology\n*Petroleum Technology,\n*Physics,\n*Physiology,\n*Statistics,\n*Zoology.");
                    break;
                case "social":
                case "social science":
                    await context.PostAsync("The Faculty of Social Science is comprised of the following Departments:");
                    await context.PostAsync("*Arabic,\n*Bengali,\n*Criminology,\n*Economics,\n*English,\n*General History,\n*International Relations,\n*Islamic History,\n*Library & Information Science,\n*Mass Communication,\n*Persian,\n*Philosophy,\n*Political Science,\n*Psychology,\n*Sindhi,\n*Shah Latif Chair,\n*Social Work,\n*Sociology,\n*Urdu,\n*Visual Studies,\n*Research Facility Centre.");
                    break;
                default:
                    await context.PostAsync("Faculty '" + name + "' not found.");
                    break;
            }
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Percentage")]
        public async Task Percentage(IDialogContext context, LuisResult luisResult)
        {
            EntityRecommendation deptName;
            string dName = string.Empty;
            if (luisResult.TryFindEntity("Department", out deptName))
            {
                dName = deptName.Entity;
            }
            switch (dName)
            {
                case "applied physics":
                    await context.PostAsync("Minimum percentage required for Applied Physic is 73.91");
                    break;
                case "biotechnology":
                    await context.PostAsync("Minimum percentage required for Biotechnology is 80.73");
                    break;
                case "chemical engineering":
                    await context.PostAsync("Minimum percentage required for Chemical Engineering is 76.09");
                    break;
                case "commerce":
                    await context.PostAsync("Minimum percentage required for Commerce is 74.91");
                    break;
                case "computer science":
                    await context.PostAsync("Minimum percentage required for Computer Science is 76.45");
                    break;
                case "software engineering":
                    await context.PostAsync("Minimum percentage required for Software Engineering is 78.26");
                    break;
                case "economics":
                    await context.PostAsync("Minimum percentage required for Economics is 66.27");
                    break;
                case "education":
                    await context.PostAsync("Minimum percentage required for Education is 66.64");
                    break;
                case "english":
                    await context.PostAsync("Minimum percentage required for English is 78.64");
                    break;
                case "food science":
                    await context.PostAsync("Minimum percentage required for Food Science is 75.00");
                    break;
                case "international relations":
                    await context.PostAsync("Minimum percentage required for International Relations is 73.91");
                    break;
                case "kubs":
                case "karachi university business school":
                    await context.PostAsync("Minimum percentage required for Karachi University Business School is 80.18");
                    break;
                case "mass communication":
                    await context.PostAsync("Minimum percentage required for Mass Communication is 77.09");
                    break;
                case "mathematics":
                    await context.PostAsync("Minimum percentage required for Mathematics is 65.64");
                    break;
                case "microbiology":
                    await context.PostAsync("Minimum percentage required for Microbiology is 78.82");
                    break;
                case "public administration":
                    await context.PostAsync("Minimum percentage required for Public Administration is 76.00");
                    break;
                case "special education":
                    await context.PostAsync("Minimum percentage required for Special Education is 62.73");
                    break;
                case "teacher education":
                    await context.PostAsync("Minimum percentage required for Teacher Education is 71.36");
                    break;
                default:
                    await context.PostAsync("Department '" + dName + "' not found.");
                    break;
            }
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("CourseOutline")]
        public async Task CourseOutline(IDialogContext context, LuisResult luisResult)
        {
            EntityRecommendation CourseOut;
            string cName = string.Empty;
            if (luisResult.TryFindEntity("Department", out CourseOut))
            {
                cName = CourseOut.Entity;
            }
            switch (cName)
            {
                case "computer science":
                    await context.PostAsync("Semester – I: BSCS-301 Introduction to Computer Science - I, BSCS-303 Mathematics - I(Calculus), BSCS-305 Statistics and Data Analysis, BSCS-307 Physics - I(General Physics), BSCS-309 English, BSCS-311 Islamic Learning & Pakistan Studies or Ethics & Pakistan Studies");
                    await context.PostAsync("Semester – II: BSCS-302 Introduction to Computer Science - II, BSCS-304 Mathematics - II(Differential Equations), BSCS-306 Probability and Statistical Methods, BSCS-308 Physics - II(Electricity and Magnetism), BSCS-310 English, BSCS-312 Urdu");
                    await context.PostAsync("Semester – III: BSCS-401 Digital Computer Design Fundamentals, BSCS-403 Assembly Language Programming, BSCS-405 Mathematics - III(Linear Algebra and Analytical Geometry), BSCS-409 Materials, Semiconductors and Devices, BSCS-411 Discrete Mathematics, BSCS-413 Object Oriented Programming");
                    await context.PostAsync("Semester – IV: BSCS-402 Data Structures, BSCS - 404 System Design with Microprocessors, BSCS - 406 Mathematics - IV(Numerical Computing), BSCS - 410 Electronics, BSCS - 412 Software Engineering & Project Management, BSCS - 414 Communication Skills and Report Writing");
                    await context.PostAsync("Semester – V: BSCS-501 Theory of Computer Science, BSCS-503 Data Communication and Networking - I, BSCS-507 Operations Research - I(Optional), BSCS-509 Database Systems, BSCS-511 Computer Organization and Architecture, BSCS-515 Artificial Intelligence, BSCS-519 Business Programming Language");
                    await context.PostAsync("Semester – VI: BSCS-502 Concepts of Operating Systems, BSCS-504 Compiler Construction - I, BSCS-506 Modeling and Simulation(Optional), BSCS-508 Operations Research - II(Optional), BSCS-512 Data Communication and Networking - II, BSCS-514 Computer Graphics, BSCS-520 Advanced Software Engineering, BSCS-522 Expert Systems");
                    await context.PostAsync("Semester – VII: BSCS-601 Theory of Operating Systems, BSCS-603 Compiler Construction - II, BSCS-607 Financial Accounting, BSCS-611 Parallel Computing(Optional), BSCS-613 Management Information System, BSCS-619 Thesis, BSCS-621 Topics of Current Interest, BSCS-633 Internet Application Development(Optional)");
                    await context.PostAsync("Semester – VIII: BSCS-604 Natural Language Processing, BSCS-606 Distributed Database Systems, BSCS-610 Design and Analysis of Algorithms, BSCS-612 Financial Management, BSCS-616 Multimedia Systems(Optional), BSCS-618 Computational Linear Algebra(Optional), BSCS-620 Thesis, BSCS-624 Project");
                    break;
                case "bba":
                    await context.PostAsync("Semester – I: BA(BBA)-301 Business English, BA(BBA)-311 Basic Mathematics, BA(BBA)-321 Human behavior, BA(BBA)-331 Principles of Management, BA(BBA)-341 Principles of Accounting, BA(BBA)-351 Computer Application in Business");
                    await context.PostAsync("Semester – II: BA(BBA)-302 Business Communication, BA(BBA)-312 Financial Accounting, BA(BBA)-322, Principles of Marketing, BA(BBA)-332 Calculus, BA(BBA)-342 Micro Economics, BA(BBA)-352 Logic");
                    await context.PostAsync("Semester – III: BA(BBA)-401 Marketing Management, BA(BBA)-411 Cost Accounting, BA(BBA)-421 Macro Economics, BA(BBA)-431 Statistics, BA(BBA)-441 Community Development, BA(BBA)-451 Islamic Studies");
                    await context.PostAsync("Semester – IV: BA(BBA)-402 International Relations, BA(BBA)-412 Introduction to Business Finance, BA(BBA)-422 Financial Institutions and Markets, BA(BBA)-432 Managerial Accounting, BA(BBA)-442 Production and Operation Management, BA(BBA)-452 Organizational Behavior");
                    await context.PostAsync("Semester – V: BA(BBA)-501 Financial Management, BA(BBA)-511 Development Economics & Economy of Pakistan, BA(BBA)-521 Supply chain Management, BA(BBA)-531 Human Resource Management, BA(BBA)-541 Pakistan Studies, BA(BBA)-551 Business Ethics and Corporate Governance");
                    await context.PostAsync("Semester – VI: BA(BBA)-502 Strategic Management, BA(BBA)-512 Business Law and Regulation, BA(BBA)-522 Business Research Methods, BA(BBA)-532 Entrepreneurship, BA(BBA)-542 History of Ideas, BA(BBA)-552 Leadership and Social Responsibility");
                    await context.PostAsync("Semester – VII: BA(BBA)-601 Decision Making and Negotiations, BA(BBA)-611 Speech Communication, BA(BBA)-621 E-Business, BA(BBA)-631 International Business, BA(BBA)-641 Corporate Performance and Planning, BA(BBA)-651 Organizational Development and TQM");
                    await context.PostAsync("Semester – VIII: BA(BBA)-602 Statistical Inference, BA(BBA)-612 Project Report, Elective I, Elective II, Elective III, Elective IV");
                    break;
                default:
                    await context.PostAsync("Department '" + cName + "' not found.");
                    break;
            }
        }

        [LuisIntent("Seats")]
        public async Task Seats(IDialogContext context, LuisResult luisResult)
        {
            Random rnd = new Random();
            int seats = rnd.Next(60, 200);
            EntityRecommendation CourseOut;
            string cName = string.Empty;
            if (luisResult.TryFindEntity("Department", out CourseOut))
            {
                cName = CourseOut.Entity;
            }
            switch (cName)
            {
                case "computer science":
                    await context.PostAsync("Seats available for Computer Science: " + seats);
                    break;
                case "bba":
                    await context.PostAsync("Seats available for BBA: " + seats);
                    break;
                case "mathematics":
                    await context.PostAsync("Seats available for Mathematics: " + seats);
                    break;
                case "mass communication":
                    await context.PostAsync("Seats available for Mass Communication: " + seats);
                    break;
                case "kubs":
                case "karachi university business school":
                    await context.PostAsync("Seats available for Karachi University Business School: " + seats);
                    break;
                default:
                    await context.PostAsync("Department '" + cName + "' not found.");
                    break;
            }
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult luisResult)
        {
            await context.PostAsync("I am a Chatbot designed to answer questions about Karachi University. So you could ask me anything about Karachi University :). Like tell me about faculties etc.");
            context.Wait(this.MessageReceived);
        }
    }

}