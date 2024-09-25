using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyWindowsFormsApp
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // Import the GetCursorPos function from user32.dll
        [DllImport("user32.dll", SetLastError = true)]

        private static extern bool GetCursorPos(out POINT lpPoint);
         
        //define Label for print location in form window

         private Label positionLabel;

        // Define the POINT structure
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        const int INPUT_MOUSE = 0;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        private  int FixedX;
        private  int FixedY;

        private int nextposptr;
        private string[] keywords;
        private int [,] nextpos;
        private int [,] Links;
        private int linkpos;
        Random random = new Random();
        private int ptr;
        public Form1()
        {
            InitializeComponent();
            MoveCursorAndStartClicking();
            OnClickTimerTick();
        }

        // Function to get the current mouse position
        private POINT GetCurrentMousePosition()
        {
            POINT point;
            if (GetCursorPos(out point))
            {
                return point;
            }
            else
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private void MoveCursorAndStartClicking()
        {    
        ptr=0;
        nextposptr=0;
        linkpos=0;
        Links=new int[,]{{230,200},{310,200},{390,200},{460,200},{540,200},{620,200},{690,200},{770,200},{850,200},{930,200},{1000,200},{1080,200},{1160,200},{1230,200},{1310,200},{230,300},{310,300},{390,300},{460,300},{540,300},{620,300},{690,300},{770,300},{850,300},{930,300},{1000,300},{1080,300},{1160,300},{1230,300},{1310,300}};
        nextpos= new int[,]{{500,750},{530,750},{570,750},{610,750},{640,750},{680,750},{720,750},{760,750},{800,750},{830,750},}; // X coordinate of the first point
        keywords=new string[]{ 
  "Climate Change", "AI Regulation", "Global Economic Outlook", "Interest Rate Hikes", "Inflation Concerns", 
  "Electric Vehicle Adoption", "Tech Layoffs", "Space Exploration", "Cybersecurity Threats", "Cryptocurrency Regulations", 
  "Social Media Policies", "Energy Crisis", "Political Elections", "Pandemic Recovery", "Supply Chain Issues", 
  "Renewable Energy Investments", "Healthcare Innovations", "Geopolitical Tensions", "Data Privacy Laws", "AI Ethics", 
  "Ukraine Conflict", "COVID-19 Variants", "Middle East Peace Talks", "China-US Relations", "NATO Expansion", 
  "Stock Market Volatility", "Federal Reserve Policies", "Global Trade Agreements", "Immigration Policies", "Digital Transformation", 
  "Green New Deal", "Tech Antitrust Laws", "5G Rollout", "Electric Grid Resilience", "Public Health Initiatives", 
  "Space Tourism", "Net Neutrality", "Ocean Conservation", "Climate Migration", "Income Inequality", 
  "Public Safety Reforms", "Digital Currency", "AI in Healthcare", "Climate Action Plans", "Affordable Housing", 
  "Corporate Tax Policies", "Global Vaccine Distribution", "Renewable Energy Subsidies", "Universal Basic Income", "Water Scarcity", 
  "Tech Privacy Laws", "Electric Car Infrastructure", "AI Bias", "Election Security", "Climate Finance", 
  "Affordable Healthcare", "Workforce Automation", "Sustainable Agriculture", "Racial Equity Initiatives", "Universal Healthcare", 
  "Climate Resilience", "Cryptocurrency Adoption", "Energy Transition", "Global Carbon Markets", "Big Tech Regulation", 
  "Digital Education", "Space Exploration Partnerships", "Climate Tech", "AI Governance", "Green Building Standards", 
  "Public Transit Modernization", "Vaccine Mandates", "Food Security", "Work from Home Policies", "Space Debris", 
  "Remote Learning", "Tech Industry Layoffs", "Carbon Capture Technology", "Climate Litigation", "Electric Airplanes", 
  "Corporate Social Responsibility", "Smart Cities", "Circular Economy", "Tech for Good", "Climate Justice", 
  "Renewable Energy Jobs", "Internet of Things (IoT)", "Sustainable Fashion", "Global Health Initiatives", "Biodiversity Conservation", 
  "Telemedicine", "Digital Identity", "Tech and Mental Health", "Energy Efficiency Standards", "Fair Trade Policies", 
  "Smart Grids", "Remote Work Trends", "AI and Ethics", "Electric Bicycles", "Cybersecurity Infrastructure",
  "Olympics 2024", "FIFA World Cup", "NBA Finals", "Super Bowl", "Tour de France", 
  "Wimbledon", "Cricket World Cup", "UEFA Champions League", "Tokyo Olympics", "Formula 1", 
  "NFL Draft", "MLB World Series", "NHL Stanley Cup", "Rugby World Cup", "UFC Fights", 
  "WrestleMania", "French Open", "US Open Tennis", "Australian Open", "The Masters Golf", 
  "Ryder Cup", "Premier League", "La Liga", "Serie A", "Bundesliga", 
  "Copa America", "Indian Premier League", "March Madness", "NCAA Football", "NASCAR Cup Series", 
  "Winter Olympics", "Summer Olympics", "Commonwealth Games", "Asian Games", "Six Nations Rugby", 
  "Ironman Triathlon", "Boston Marathon", "New York Marathon", "London Marathon", "MMA Events", 
  "Boxing Championship", "World Athletics Championships", "Diamond League", "FIFA Club World Cup", "Ryder Cup Golf", 
  "Golf PGA Tour", "Tennis ATP Finals", "Davis Cup", "FA Cup", "Europa League", 
  "Wimbledon Final", "Rugby Sevens", "Tour of Britain", "Cricket Ashes Series", "ICC T20 World Cup", 
  "Women’s World Cup", "FIBA Basketball World Cup", "Motorsport Events", "America’s Cup Sailing", "Horse Racing",
   // Education
    "education reform", "school funding", "student loans", "online learning", 
    "university rankings", "education policies", "distance education", "STEM education", 
    "teacher shortages", "curriculum changes", "college admissions", "graduation rates", 
    "special education", "student debt", "higher education", "academic freedom", 
    "education budget", "tuition fees", "public schools", "private schools", 
    "early childhood education", "vocational training", "charter schools", "homeschooling", 
    "digital education", "scholarships", "research grants", "adult education", 
    "learning disabilities", "educational equity", "student well-being", 
    "school closures", "classroom technology", "teacher training", "school infrastructure", 
    "education for refugees", "academic scholarships", "school uniforms", 
    "parental involvement", "textbook costs", "education innovation", "lifelong learning", 
    "education standards", "blended learning", "education inequality", 
    "school safety", "bullying prevention", "college rankings", "school choice", 
    "education for the disabled", "student engagement", "teacher evaluations", 
    "higher education funding", "student-teacher ratio", "civic education", 
    "bilingual education", "education research", "teacher salaries", 
    "school lunch programs", "education accessibility", "digital literacy", 
    "MOOCs", "open educational resources", "student exchange programs", "free education", 
    "community colleges", "tuition hikes", "school accreditation", "education law", 
    "distance learning platforms", "school counseling", "career guidance", 
    "class size reduction", "teacher retention", "education workforce", "school reforms", 
    
    // Politics
    "election results", "voter turnout", "political debates", "foreign policy", 
    "government reforms", "political parties", "campaign finance", "legislative bills", 
    "executive orders", "parliamentary proceedings", "democracy", "authoritarianism", 
    "political scandals", "voting rights", "political campaigns", "governance", 
    "cabinet reshuffle", "law enforcement", "judiciary reforms", "international relations", 
    "human rights", "civil rights movements", "populism", "political ideologies", 
    "public opinion", "presidential debates", "coalition governments", 
    "foreign aid policies", "diplomatic relations", "geopolitical tensions", 
    "political corruption", "lobbying", "election integrity", "federalism", 
    "constitutional amendments", "civic engagement", "government transparency", 
    "political polarization", "political protests", "voter suppression", 
    "political endorsements", "political rhetoric", "partisan politics", 
    "political accountability", "international diplomacy", "national security", 
    "cybersecurity", "military alliances", "global governance", 
    "transparency in governance", "whistleblower protections", "surveillance laws", 
    "political asylum", "freedom of speech", "political extremism", 
    "environmental policy", "immigration policy", "border security", "trade agreements", 
    "peace treaties", "conflict resolution", "arms control", "nuclear disarmament", 
    "war crimes", "military intervention", "defense budget", "political lobbying", 
    
    // Economy
    "economic growth", "unemployment rates", "inflation", "interest rates", 
    "GDP", "foreign exchange", "stock market", "trade deficits", 
    "budget deficit", "national debt", "income inequality", "monetary policy", 
    "fiscal policy", "tax reform", "trade tariffs", "public spending", 
    "investment banking", "venture capital", "economic recovery", "recession", 
    "housing market", "consumer confidence", "corporate tax rates", "wage growth", 
    "minimum wage", "labor market", "globalization", "economic sanctions", 
    "oil prices", "cryptocurrency", "blockchain", "e-commerce", 
    "financial regulation", "credit ratings", "sovereign debt", "currency devaluation", 
    "trade unions", "outsourcing", "supply chain disruption", "economic stimulus", 
    "poverty reduction", "income tax", "capital gains tax", "wealth management", 
    "federal reserve", "central banks", "private equity", "pension funds", 
    "social security", "employment benefits", "universal basic income", 
    "foreign direct investment", "public-private partnerships", "venture funding", 
    "economic inequality", "healthcare costs", "gig economy", "startup ecosystem", 
    "green economy", "renewable energy investment", "climate finance", 
    "global trade", "foreign investments", "public infrastructure", "debt relief", 
    "urbanization", "industrialization", "digital economy", "financial literacy", 
    
    // National
    "national elections", "border control", "immigration laws", "gun control", 
    "civil rights", "criminal justice reform", "public health", "national security", 
    "national parks", "climate change policy", "wildlife conservation", "energy independence", 
    "drug policy", "healthcare reform", "transportation infrastructure", 
    "homeland security", "disaster relief", "police reform", "social welfare", 
    "affordable housing", "criminal justice", "public safety", "mental health services", 
    "natural disasters", "national defense", "firearm legislation", "state sovereignty", 
    "environmental regulation", "military spending", "veteran affairs", 
    "census data", "tax cuts", "public transportation", "federal budget", 
    "cyber warfare", "law enforcement reform", "space exploration", 
    "domestic terrorism", "rural development", "public education", "water rights", 
    "agricultural policy", "energy policy", "internet privacy", "civil liberties", 
    "public broadcasting", "judicial appointments", "family planning", 
    "domestic violence", "child welfare", "foster care", "drug addiction", 
    "food security", "employment laws", "wage discrimination", "health insurance", 
    "privacy laws", "telecommunications", "patriotism", "public defenders", 
    
    // International
    "international trade", "global economy", "UN resolutions", "climate agreements", 
    "foreign aid", "global warming", "refugee crisis", "humanitarian aid", 
    "international diplomacy", "world bank", "international sanctions", 
    "global health", "food security", "nuclear weapons", "terrorism", 
    "peacekeeping missions", "global poverty", "pandemic response", "WHO guidelines", 
    "international law", "war crimes", "geopolitical conflicts", "international courts", 
    "G20 summit", "world trade organization", "global energy crisis", "space exploration", 
    "ocean conservation", "renewable energy", "sustainable development goals", 
    "human trafficking", "global security", "international human rights", 
    "digital privacy", "trade wars", "international labor laws", "foreign policy", 
    "arms trade", "global governance", "climate refugees", "food shortages", 
    "international financial markets", "wildlife protection", "nuclear disarmament", 
    "space race", "oil exploration", "global supply chains", "marine pollution", 
    "internet governance", "multilateralism", "international relations", "humanitarian law", 
    "non-governmental organizations", "world heritage sites", "foreign reserves", 
    "global conflict zones", "public diplomacy", "international development", 
    "intellectual property rights", "foreign currency reserves", "cyber espionage"
            };
        }

        private void clickit(){
            INPUT[] inputs = new INPUT[2];
             // Mouse down
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dx = (int)(FixedX * (65535.0 / Screen.PrimaryScreen.Bounds.Width)); // Convert to absolute coordinates
            inputs[0].mi.dy = (int)(FixedY * (65535.0 / Screen.PrimaryScreen.Bounds.Height)); // Convert to absolute coordinates
            inputs[0].mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN;
            inputs[0].mi.mouseData = 0;
            inputs[0].mi.time = 0;
            inputs[0].mi.dwExtraInfo = IntPtr.Zero;

            // Mouse up
            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dx = (int)(FixedX * (65535.0 / Screen.PrimaryScreen.Bounds.Width)); // Convert to absolute coordinates
            inputs[1].mi.dy = (int)(FixedY * (65535.0 / Screen.PrimaryScreen.Bounds.Height)); // Convert to absolute coordinates
            inputs[1].mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP;
            inputs[1].mi.mouseData = 0;
            inputs[1].mi.time = 0;
            inputs[1].mi.dwExtraInfo = IntPtr.Zero;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private void performtask(){
            for(int i=0;i<5;i++){
            var position = GetCurrentMousePosition();
            int X=position.X;
            int Y=position.Y;
            FixedX=nextpos[nextposptr,0];
            FixedY=nextpos[nextposptr,1];
            ptr=random.Next(0,keywords.Length-1);
            INPUT[] inputs = new INPUT[2];
            SetCursorPos(FixedX, FixedY);
            clickit();
            Thread.Sleep(500);

            SetCursorPos(392, 16);
            clickit();

            SendKeys.SendWait(keywords[ptr]);
            SendKeys.SendWait("{ENTER}");

            SetCursorPos(354, 16);
            clickit();

            SetCursorPos(position.X, position.Y);
            nextposptr=(nextposptr+1)%5;
            Thread.Sleep(2500);
            }

        }

        private void setup(){
            SetCursorPos(15,750);
            clickit();
            Thread.Sleep(1000);
            SetCursorPos(700, 330);
            clickit();
            Thread.Sleep(5000);
            for(int i=0;i<5;i++){
                SetCursorPos(Links[linkpos,0],Links[linkpos,1]);
                clickit();
                Thread.Sleep(5000);
                SetCursorPos(310,750);
                clickit();
                Thread.Sleep(1000);
                linkpos++;
            }
            SetCursorPos(1340,20);
            clickit();

        }
        private void closetabs(){
            SetCursorPos(1340,20);
            for(int i=0;i<8;i++){
                clickit();
                Thread.Sleep(1000);
            }
        }
        private void task(){
            setup();
            Thread.Sleep(1000);
            for(int i=0;i<1;i++){
                performtask();
            }
            closetabs();
        }
        private void OnClickTimerTick()
        {
           SetCursorPos(1240,20);
           clickit();
          Thread.Sleep(1000);
           for(int i=0;i<4;i++){
               task();
               Thread.Sleep(2000);
           }

           SetCursorPos(430,750);
           clickit();
           Thread.Sleep(500);
           SetCursorPos(1340,20);
           clickit();
        }
    }
}