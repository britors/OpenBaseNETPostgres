using Npgsql;


namespace OpenBaseNET.Infra.Resilience.Database.Postres.ExceptionPredicate;

internal static class PostgresExceptionPredicate
{
    internal static bool ShouldRetryOn(PostgresException exception)
    {
        return exception.SqlState switch
        {
            SerializationFailure or // Serialization Failure
                ActiveSQLTransaction or // Active SQL Transaction
                AdminShutdown or // Admin Shutdown
                IOError or // IO Error
                TooManyConnections or // Too Many Connections
                ObjectNotInPrerequisiteState or // Object Not In Prerequisite State
                ConnectionException or // Connection Exception
                ConnectionDoesNotExist or // Connection Does Not Exist
                ConnectionFailure or // Connection Failure
                SQLClientUnableToEstablishConnection or // SQL Client Unable to Establish Connection
                TransactionResolutionUnknown or // Transaction Resolution Unknown
                ConnectionNameInUse or // Connection Name in Use
                InvalidConnectionName or // Invalid Connection Name
                SQLStatementNameTooLong or // SQL Statement Name Too Long
                SQLConnectionAlreadyExists or // SQL Connection Already Exists
                SQLServerConnectionLimitExceeded or // SQL Server Connection Limit Exceeded
                ProtocolViolation or // Protocol Violation
                CannotConnectNow or // Cannot Connect Now
                SQLServerConnectionRejectedEstablishmentOfSQLConnectionTransaction or // SQL Server Connection Rejected Establishment of SQL Connection Transaction
                DeprecatedFeature or // Deprecated Feature
                SQLRoutineException or // SQL Routine Exception
                SQLRoutineSQLState or // SQL Routine SQLSTATE
                SQLRoutineExceptionRaiseException or // SQL Routine Exception: Raise Exception
                SQLRoutineExceptionNoDataFound or // SQL Routine Exception: No Data Found
                SQLRoutineModifyingSQLDataNotPermitted or // SQL Routine Modifying SQL Data Not Permitted
                SQLRoutineProhibitedSQLStatementAttempted or // SQL Routine Prohibited SQL Statement Attempted
                SQLRoutineReadingSQLDataNotPermitted or // SQL Routine Reading SQL Data Not Permitted
                SQLRoutineFunctionExecutedNoReturnStatement or // SQL Routine Function Executed No Return Statement
                InvalidCursorName or // Invalid Cursor Name
                SavepointException or // Savepoint Exception
                SavepointExceptionInvalidSavepointSpecification or // Savepoint Exception: Invalid Savepoint Specification
                SavepointExceptionTooManySavepoints or // Savepoint Exception: Too Many Savepoints
                SavepointExceptionSavepointNotEstablished or // Savepoint Exception: Savepoint Not Established
                TransactionRollback or // Transaction Rollback
                TransactionIntegrityConstraintViolation or // Transaction Integrity Constraint Violation
                StatementCompletionUnknown or // Statement Completion Unknown
                DeadlockDetected or // Deadlock Detected
                SyntaxErrorOrAccessRuleViolation or // Syntax Error or Access Rule Violation
                SyntaxError or // Syntax Error
                InsufficientPrivilege or // Insufficient Privilege
                CannotCoerce or // Cannot Coerce
                GroupingError or // Grouping Error
                WindowingError or // Windowing Error
                InvalidRecursion or // Invalid Recursion
                InvalidForeignKey or // Invalid Foreign Key
                InvalidName or // Invalid Name
                NameTooLong or // Name Too Long
                ReservedName or // Reserved Name
                DatatypeMismatch or // Datatype Mismatch
                IndeterminateDatatype or // Indeterminate Datatype
                CollationMismatch or // Collation Mismatch
                IndeterminateCollation or // Indeterminate Collation
                WrongObjectType or // Wrong Object Type
                UndefinedColumn or // Undefined Column
                UndefinedFunction or // Undefined Function
                UndefinedTable or // Undefined Table
                UndefinedParameter or // Undefined Parameter
                UndefinedObject or // Undefined Object
                DuplicateColumn or // Duplicate Column
                DuplicateCursor or // Duplicate Cursor
                DuplicateDatabase or // Duplicate Database
                DuplicateFunction or // Duplicate Function
                DuplicatePreparedStatement or // Duplicate Prepared Statement
                DuplicateSchema or // Duplicate Schema
                DuplicateTable or // Duplicate Table
                DuplicateAlias or // Duplicate Alias
                DuplicateObject or // Duplicate Object
                AmbiguousColumn or // Ambiguous Column
                AmbiguousFunction or // Ambiguous Function
                AmbiguousParameter or // Ambiguous Parameter
                AmbiguousAlias or // Ambiguous Alias
                InvalidColumnReference or // Invalid Column Reference
                InvalidColumnDefinition or // Invalid Column Definition
                InvalidCursorDefinition or // Invalid Cursor Definition
                InvalidDatabaseDefinition or // Invalid Database Definition
                InvalidFunctionDefinition or // Invalid Function Definition
                InvalidPreparedStatementDefinition or // Invalid Prepared Statement Definition
                InvalidSchemaDefinition or // Invalid Schema Definition
                InvalidTableDefinition or // Invalid Table Definition
                InvalidObjectDefinition or // Invalid Object Definition
                WithCheckOptionViolation or // With Check Option Violation
                InsufficientResources or // Insufficient Resources
                DiskFull or // Disk Full
                OutOfMemory or // Out of Memory
                ConfigurationLimitExceeded or // Configuration Limit Exceeded
                ProgramLimitExceeded or // Program Limit Exceeded
                StatementTooComplex or // Statement Too Complex
                TooManyColumns or // Too Many Columns
                TooManyArguments or // Too Many Arguments
                ObjectInUse or // Object In Use
                CantChangeRuntimeParam or // Cant Change Runtime Param
                LockNotAvailable or // Lock Not Available
                UnsafeNewEnumValueUsage or // Unsafe New Enum Value Usage
                OperatorIntervention or // Operator Intervention
                QueryCanceled or // Query Canceled
                DatabaseDropped or // Database Dropped
                SystemError or // System Error
                UndefinedFile or // Undefined File
                DuplicateFile or // Duplicate File
                ConfigFileError or // Config File Error
                LockFileExists or // Lock File Exists
                FDWError or // FDW Error
                FDWColumnNameNotFound or // FDW Column Name Not Found
                FDWDynamicParameterValueNeeded or // FDW Dynamic Parameter Value Needed
                FDWFunctionSequenceError or // FDW Function Sequence Error
                FDWInconsistentDescriptorInformation or // FDW Inconsistent Descriptor Information
                FDWInvalidAttributeValue or // FDW Invalid Attribute Value
                FDWInvalidColumnName or // FDW Invalid Column Name
                FDWInvalidColumnNumber or // FDW Invalid Column Number
                FDWInvalidDataType or // FDW Invalid Data Type
                FDWInvalidDataTypeDescriptors or // FDW Invalid Data Type Descriptors
                FDWInvalidDescriptorFieldIdentifier or // FDW Invalid Descriptor Field Identifier
                FDWInvalidHandle or // FDW Invalid Handle
                FDWInvalidOptionIndex or // FDW Invalid Option Index
                FDWInvalidOptionName or // FDW Invalid Option Name
                FDWInvalidStringLengthOrBufferLength or // FDW Invalid String Length Or Buffer Length
                FDWInvalidStringFormat or // FDW Invalid String Format
                FDWInvalidUseOfNullPointer or // FDW Invalid Use Of Null Pointer
                FDWTooManyHandles or // FDW Too Many Handles
                FDWOutOfMemory or // FDW Out Of Memory
                FDWNoSchemas or // FDW No Schemas
                FDWOptionNameNotFound or // FDW Option Name Not Found
                FDWReplyHandle or // FDW Reply Handle
                FDWSchemaNotFound or // FDW Schema Not Found
                FDWTableNotFound or // FDW Table Not Found
                FDWUnableToCreateExecution or // FDW Unable To Create Execution
                FDWUnableToCreateReply or // FDW Unable To Create Reply
                FDWUnableToEstablishConnection or // FDW Unable To Establish Connection
                PLPGSQLError or // PLPGSQL Error
                RaiseException or // Raise Exception
                NoDataFound or // No Data Found
                TooManyRows or // Too Many Rows
                InternalError or // Internal Error
                DataCorrupted or // Data Corrupted
                IndexCorrupted => true,
            _ => false
        };
    }

    #region Constants

    private const string SerializationFailure = "40001";
    private const string ActiveSQLTransaction = "25001";
    private const string AdminShutdown = "57P01";
    private const string IOError = "58030";
    private const string TooManyConnections = "53300";
    private const string ObjectNotInPrerequisiteState = "55000";
    private const string ConnectionException = "08000";
    private const string ConnectionDoesNotExist = "08003";
    private const string ConnectionFailure = "08006";
    private const string SQLClientUnableToEstablishConnection = "08001";
    private const string TransactionResolutionUnknown = "08007";
    private const string ConnectionNameInUse = "08002";
    private const string InvalidConnectionName = "08014";
    private const string SQLStatementNameTooLong = "08003";
    private const string SQLConnectionAlreadyExists = "08006";
    private const string SQLServerConnectionLimitExceeded = "08001";
    private const string ProtocolViolation = "08P01";
    private const string CannotConnectNow = "57P03";
    private const string SQLServerConnectionRejectedEstablishmentOfSQLConnectionTransaction = "08004";
    private const string DeprecatedFeature = "01P01";
    private const string SQLRoutineException = "2F002";
    private const string SQLRoutineSQLState = "2F003";
    private const string SQLRoutineExceptionRaiseException = "2F004";
    private const string SQLRoutineExceptionNoDataFound = "2F005";
    private const string SQLRoutineModifyingSQLDataNotPermitted = "2F002";
    private const string SQLRoutineProhibitedSQLStatementAttempted = "2F003";
    private const string SQLRoutineReadingSQLDataNotPermitted = "2F004";
    private const string SQLRoutineFunctionExecutedNoReturnStatement = "2F005";
    private const string InvalidCursorName = "34000";
    private const string SavepointException = "3B000";
    private const string SavepointExceptionInvalidSavepointSpecification = "3B001";
    private const string SavepointExceptionTooManySavepoints = "3B002";
    private const string SavepointExceptionSavepointNotEstablished = "3B003";
    private const string TransactionRollback = "40000";
    private const string TransactionIntegrityConstraintViolation = "40002";
    private const string StatementCompletionUnknown = "40003";
    private const string DeadlockDetected = "40P01";
    private const string SyntaxErrorOrAccessRuleViolation = "42000";
    private const string SyntaxError = "42601";
    private const string InsufficientPrivilege = "42501";
    private const string CannotCoerce = "42846";
    private const string GroupingError = "42803";
    private const string WindowingError = "42P20";
    private const string InvalidRecursion = "42P19";
    private const string InvalidForeignKey = "42830";
    private const string InvalidName = "42602";
    private const string NameTooLong = "42622";
    private const string ReservedName = "42939";
    private const string DatatypeMismatch = "42804";
    private const string IndeterminateDatatype = "42P18";
    private const string CollationMismatch = "42P21";
    private const string IndeterminateCollation = "42P22";
    private const string WrongObjectType = "42809";
    private const string UndefinedColumn = "42703";
    private const string UndefinedFunction = "42883";
    private const string UndefinedTable = "42P01";
    private const string UndefinedParameter = "42P02";
    private const string UndefinedObject = "42704";
    private const string DuplicateColumn = "42701";
    private const string DuplicateCursor = "42P03";
    private const string DuplicateDatabase = "42P04";
    private const string DuplicateFunction = "42723";
    private const string DuplicatePreparedStatement = "42P05";
    private const string DuplicateSchema = "42P06";
    private const string DuplicateTable = "42P07";
    private const string DuplicateAlias = "42712";
    private const string DuplicateObject = "42710";
    private const string AmbiguousColumn = "42702";
    private const string AmbiguousFunction = "42725";
    private const string AmbiguousParameter = "42P08";
    private const string AmbiguousAlias = "42P09";
    private const string InvalidColumnReference = "42P10";
    private const string InvalidColumnDefinition = "42611";
    private const string InvalidCursorDefinition = "42P11";
    private const string InvalidDatabaseDefinition = "42P12";
    private const string InvalidFunctionDefinition = "42P13";
    private const string InvalidPreparedStatementDefinition = "42P14";
    private const string InvalidSchemaDefinition = "42P15";
    private const string InvalidTableDefinition = "42P16";
    private const string InvalidObjectDefinition = "42P17";
    private const string WithCheckOptionViolation = "44000";
    private const string InsufficientResources = "53000";
    private const string DiskFull = "53100";
    private const string OutOfMemory = "53200";
    private const string ConfigurationLimitExceeded = "53400";
    private const string ProgramLimitExceeded = "54000";
    private const string StatementTooComplex = "54001";
    private const string TooManyColumns = "54011";
    private const string TooManyArguments = "54023";
    private const string ObjectInUse = "55006";
    private const string CantChangeRuntimeParam = "55P02";
    private const string LockNotAvailable = "55P03";
    private const string UnsafeNewEnumValueUsage = "55P04";
    private const string OperatorIntervention = "57000";
    private const string QueryCanceled = "57014";
    private const string DatabaseDropped = "57P04";
    private const string SystemError = "58000";
    private const string UndefinedFile = "58P01";
    private const string DuplicateFile = "58P02";
    private const string ConfigFileError = "F0000";
    private const string LockFileExists = "F0001";
    private const string FDWError = "HV000";
    private const string FDWColumnNameNotFound = "HV005";
    private const string FDWDynamicParameterValueNeeded = "HV002";
    private const string FDWFunctionSequenceError = "HV010";
    private const string FDWInconsistentDescriptorInformation = "HV021";
    private const string FDWInvalidAttributeValue = "HV024";
    private const string FDWInvalidColumnName = "HV007";
    private const string FDWInvalidColumnNumber = "HV008";
    private const string FDWInvalidDataType = "HV004";
    private const string FDWInvalidDataTypeDescriptors = "HV006";
    private const string FDWInvalidDescriptorFieldIdentifier = "HV091";
    private const string FDWInvalidHandle = "HV00B";
    private const string FDWInvalidOptionIndex = "HV00C";
    private const string FDWInvalidOptionName = "HV00D";
    private const string FDWInvalidStringLengthOrBufferLength = "HV090";
    private const string FDWInvalidStringFormat = "HV00A";
    private const string FDWInvalidUseOfNullPointer = "HV009";
    private const string FDWTooManyHandles = "HV014";
    private const string FDWOutOfMemory = "HV001";
    private const string FDWNoSchemas = "HV00P";
    private const string FDWOptionNameNotFound = "HV00J";
    private const string FDWReplyHandle = "HV00K";
    private const string FDWSchemaNotFound = "HV00Q";
    private const string FDWTableNotFound = "HV00R";
    private const string FDWUnableToCreateExecution = "HV00L";
    private const string FDWUnableToCreateReply = "HV00M";
    private const string FDWUnableToEstablishConnection = "HV00N";
    private const string PLPGSQLError = "P0000";
    private const string RaiseException = "P0001";
    private const string NoDataFound = "P0002";
    private const string TooManyRows = "P0003";
    private const string InternalError = "XX000";
    private const string DataCorrupted = "XX001";
    private const string IndexCorrupted = "XX002";
    #endregion
}