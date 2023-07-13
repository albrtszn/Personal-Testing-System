using Personal_Testing_System.Services;

namespace Personal_Testing_System.Services
{
    public class MasterService
    {
        private AnswerService answerService;
        private EmployeeAnswerService employeeAnswerService;
        private EmployeeMatchingService employeeMatchingService;
        private EmployeeService employeeService;
        private EmployeeSubsequenceService employeeSubsequenceService;
        private FirstPartService firstPartService;
        private QuestionService questionService;
        private QuestionTypeService questionTypeService;
        private SecondPartService secondPartService;
        private SubdivisionService subdivisionService;
        private SubsequenceService subsequenceService;
        private TestPurposeService testPurposeService;
        private TestService testService;
        private CompetenceService competenceService;
        private AdminService adminService;
        private ResultService resultService;
        private EmployeeResultService employeeResultService;

        public MasterService(AnswerService _answerService, EmployeeAnswerService _employeeAnswerService,
                       EmployeeMatchingService _employeeMatchingService, EmployeeService _employeeService,
                       EmployeeSubsequenceService _employeeSubsequenceService, FirstPartService _firstPartService,
                       QuestionService _questionService, QuestionTypeService _questionTypeServiceService, SecondPartService _secondPartService,
                       SubdivisionService _subdivisionPartService, SubsequenceService _subsequenceService,
                       TestPurposeService _testPurposeService, TestService _testService, 
                       CompetenceService _competenceService, AdminService _adminService,
                       ResultService _resultService ,EmployeeResultService _employeeResultService)
        {
            answerService = _answerService;
            employeeAnswerService = _employeeAnswerService;
            employeeMatchingService = _employeeMatchingService;
            employeeService = _employeeService;
            employeeSubsequenceService = _employeeSubsequenceService;
            firstPartService = _firstPartService;
            questionService = _questionService;
            questionTypeService = _questionTypeServiceService;
            secondPartService = _secondPartService;
            subdivisionService = _subdivisionPartService;
            subsequenceService = _subsequenceService;
            testPurposeService = _testPurposeService;
            testService = _testService;
            competenceService = _competenceService;
            adminService = _adminService;
            resultService = _resultService;
            employeeResultService = _employeeResultService;
        } 
        //public UserService Users { get { return ; } }

        public AnswerService Answer { get { return answerService; } }
        public EmployeeAnswerService EmployeeAnswer { get { return employeeAnswerService; } }
        public EmployeeMatchingService EmployeeMatching { get { return employeeMatchingService; } }
        public EmployeeService Employee { get { return employeeService; } }
        public EmployeeSubsequenceService EmployeeSubsequence { get { return employeeSubsequenceService; } }
        public FirstPartService FirstPart { get { return firstPartService; } }
        public QuestionService Question { get { return questionService; } }
        public QuestionTypeService QuestionType { get { return questionTypeService; } }
        public SecondPartService SecondPart { get { return secondPartService; } }
        public SubdivisionService Subdivision { get { return subdivisionService; } }
        public SubsequenceService Subsequence { get { return subsequenceService; } }
        public TestPurposeService TestPurpose { get { return testPurposeService; } }
        public TestService Test { get { return testService; } }
        public CompetenceService TestType { get { return competenceService; } }
        public AdminService Admin { get { return adminService; } }
        public ResultService Result { get { return resultService; } }
        public EmployeeResultService EmployeeResult { get { return employeeResultService; } }
    }
}
