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
        private QuestionsInTestService questionsInTestService;
        private QuestionTypeService questionTypeService;
        private SecondPartService secondPartService;
        private SubdivisionService subdivisionService;
        private SubsequenceService subsequenceService;
        private TestPurposeService testPurposeService;
        private TestResultService testResultService;
        private TestService testService;
        private TestTypeService testTypeService;

        public MasterService(AnswerService _answerService, EmployeeAnswerService _employeeAnswerService,
                       EmployeeMatchingService _employeeMatchingService, EmployeeService _employeeService,
                       EmployeeSubsequenceService _employeeSubsequenceService, FirstPartService _firstPartService,
                       QuestionService _questionService, QuestionsInTestService _questionsInTestService,
                       QuestionTypeService _questionTypeServiceService, SecondPartService _secondPartService,
                       SubdivisionService _subdivisionPartService, SubsequenceService _subsequenceService,
                       TestPurposeService _testPurposeService, TestResultService _testResultService,
                       TestService _testService, TestTypeService _testTypeService)
        {
            answerService = _answerService;
            employeeAnswerService = _employeeAnswerService;
            employeeMatchingService = _employeeMatchingService;
            employeeService = _employeeService;
            employeeSubsequenceService = _employeeSubsequenceService;
            firstPartService = _firstPartService;
            questionService = _questionService;
            questionsInTestService = _questionsInTestService;
            questionTypeService = _questionTypeServiceService;
            secondPartService = _secondPartService;
            subdivisionService = _subdivisionPartService;
            subsequenceService = _subsequenceService;
            testPurposeService = _testPurposeService;
            testResultService = _testResultService;
            testService = _testService;
            testTypeService = _testTypeService;
        } 
        //public UserService Users { get { return ; } }

        public AnswerService Answer { get { return answerService; } }
        public EmployeeAnswerService EmployeeAnswer { get { return employeeAnswerService; } }
        public EmployeeMatchingService EmployeeMatching { get { return employeeMatchingService; } }
        public EmployeeService Employee { get { return employeeService; } }
        public EmployeeSubsequenceService EmployeeSubsequence { get { return employeeSubsequenceService; } }
        public FirstPartService FirstPart { get { return firstPartService; } }
        public QuestionService Question { get { return questionService; } }
        public QuestionsInTestService QuestionsInTest { get { return questionsInTestService; } }
        public QuestionTypeService QuestionTypeService { get { return questionTypeService; } }
        public SecondPartService SecondPart { get { return secondPartService; } }
        public SubdivisionService Subdivision { get { return subdivisionService; } }
        public SubsequenceService Subsequence { get { return subsequenceService; } }
        public TestPurposeService TestPurpose { get { return testPurposeService; } }
        public TestResultService TestResult { get { return testResultService; } }
        public TestService Test { get { return testService; } }
        public TestTypeService TestType { get { return testTypeService; } }
    }
}
