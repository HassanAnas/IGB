using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IGB.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Curriculums",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurriculumId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurriculumId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RelationshipWithStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegularIrregular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraCaring = table.Column<bool>(type: "bit", nullable: true),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherEmployeeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contract = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: true),
                    IsTutor = table.Column<bool>(type: "bit", nullable: true),
                    IsStudent = table.Column<bool>(type: "bit", nullable: true),
                    IsFirstGuardian = table.Column<bool>(type: "bit", nullable: true),
                    IsSecondGuardian = table.Column<bool>(type: "bit", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProfileUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsFirstGuardianUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsSecondGuardianUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsSpecialiitiesUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsEducationUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsExperienceUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsDocumentUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsContracttUpdated = table.Column<bool>(type: "bit", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: true),
                    Update = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTopics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopics_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GradeCourses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GradeCourses_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdminDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminDocuments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseBookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseBookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: true),
                    TutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StudentRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TutorRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSubmited = table.Column<bool>(type: "bit", nullable: false),
                    IsAdminApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsAdminRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsTutorApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsTutorRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Smooth = table.Column<bool>(type: "bit", nullable: false),
                    Marker = table.Column<bool>(type: "bit", nullable: false),
                    Detail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBookings_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseBookings_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseBookings_AspNetUsers_TutorId",
                        column: x => x.TutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseBookings_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorDocuments_AspNetUsers_TutorId",
                        column: x => x.TutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorEducations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UniversityLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntermediateLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherIntermediateLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatriculationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherMatriculationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Faculty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartYear = table.Column<int>(type: "int", nullable: true),
                    EndYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorEducations_AspNetUsers_TutorId",
                        column: x => x.TutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorExperiences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InstituteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurriculumId = table.Column<long>(type: "bigint", nullable: true),
                    CurriculumName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartYear = table.Column<int>(type: "int", nullable: true),
                    EndYear = table.Column<int>(type: "int", nullable: true),
                    IsEndYear = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorExperiences_AspNetUsers_TutorId",
                        column: x => x.TutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorExperiences_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorExperiences_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorSpecialitys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurriculumId = table.Column<long>(type: "bigint", nullable: true),
                    CurriculumName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpLevel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorSpecialitys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorSpecialitys_AspNetUsers_TutorId",
                        column: x => x.TutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorSpecialitys_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorSpecialitys_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorSpecialitys_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonBookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonBookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CourseBookingId = table.Column<long>(type: "bigint", nullable: true),
                    LessonBookingInitiatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicCovered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstTimeSlot = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SecondTimeSlot = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThirdTimeSlot = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    SelectedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TutorRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TutorClassStartAccess = table.Column<bool>(type: "bit", nullable: false),
                    StudentClassStartAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsSubmited = table.Column<bool>(type: "bit", nullable: false),
                    IsAdminApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsAdminRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsTutorApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsTutorRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsStudentApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsStudentRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsSessionInitiated = table.Column<bool>(type: "bit", nullable: false),
                    IsSessionEnded = table.Column<bool>(type: "bit", nullable: false),
                    SessionInitiatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionEndeddBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    SessionTutorTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    SessionStudentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsStudentPresent = table.Column<bool>(type: "bit", nullable: false),
                    StudentComingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentCheckoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsTutorPresent = table.Column<bool>(type: "bit", nullable: false),
                    TutorComingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TutorCheckoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditCharged = table.Column<double>(type: "float", nullable: true),
                    EstimatedCreditCharged = table.Column<double>(type: "float", nullable: true),
                    StudentScore = table.Column<double>(type: "float", nullable: true),
                    TutorScore = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonBookings_CourseBookings_CourseBookingId",
                        column: x => x.CourseBookingId,
                        principalTable: "CourseBookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentCredits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Credit = table.Column<double>(type: "float", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseBookingId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCredits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCredits_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentCredits_CourseBookings_CourseBookingId",
                        column: x => x.CourseBookingId,
                        principalTable: "CourseBookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentFeedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LessonBookingId = table.Column<long>(type: "bigint", nullable: true),
                    UnderstandingAndEngagement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderstandingAndEngagementScore = table.Column<double>(type: "float", nullable: true),
                    DidTutorExplainTopic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidTutorExplainTopicScore = table.Column<double>(type: "float", nullable: true),
                    DidTutorClearDoubtToday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidTutorClearDoubtTodayScore = table.Column<double>(type: "float", nullable: true),
                    DidTutorClearDoubtPrevious = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DidTutorClearDoubtPreviousScore = table.Column<double>(type: "float", nullable: true),
                    RateTutorTeaching = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RateTutorTeachingScore = table.Column<double>(type: "float", nullable: true),
                    FinalScore = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPending = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFeedbacks_LessonBookings_LessonBookingId",
                        column: x => x.LessonBookingId,
                        principalTable: "LessonBookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorFeedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LessonBookingId = table.Column<long>(type: "bigint", nullable: true),
                    PreviousHomeWorkDone = table.Column<bool>(type: "bit", nullable: false),
                    PreviousHomeWorkDoneScore = table.Column<double>(type: "float", nullable: true),
                    PreviousHomeWorkDiscussed = table.Column<bool>(type: "bit", nullable: false),
                    TopicCoveredToday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicUnderstandingLevel = table.Column<double>(type: "float", nullable: true),
                    GradePrediction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradePredictionScore = table.Column<double>(type: "float", nullable: true),
                    AverageGradePrediction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MentalSkills = table.Column<double>(type: "float", nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObtainedScore = table.Column<double>(type: "float", nullable: true),
                    TotalScore = table.Column<double>(type: "float", nullable: true),
                    Percentage = table.Column<double>(type: "float", nullable: true),
                    PercentageScore = table.Column<double>(type: "float", nullable: true),
                    TestFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NextHomeworkFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NextHomework = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Announcement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPending = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    FinalScore = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorFeedbacks_LessonBookings_LessonBookingId",
                        column: x => x.LessonBookingId,
                        principalTable: "LessonBookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AllNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NewUserRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseBookingId = table.Column<long>(type: "bigint", nullable: true),
                    CourseBookingIdForLesson = table.Column<long>(type: "bigint", nullable: true),
                    LessonBookingId = table.Column<long>(type: "bigint", nullable: true),
                    LessonBookingStartId = table.Column<long>(type: "bigint", nullable: true),
                    TutorFeedbackId = table.Column<long>(type: "bigint", nullable: true),
                    StudentFeedbackId = table.Column<long>(type: "bigint", nullable: true),
                    ForAdmin = table.Column<bool>(type: "bit", nullable: false),
                    ForTutor = table.Column<bool>(type: "bit", nullable: false),
                    ForStudent = table.Column<bool>(type: "bit", nullable: false),
                    ForTutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ForStudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsReadByAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsReadByTutor = table.Column<bool>(type: "bit", nullable: false),
                    IsReadByStudent = table.Column<bool>(type: "bit", nullable: false),
                    IsApprovedByAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsApprovedByTutor = table.Column<bool>(type: "bit", nullable: false),
                    IsApprovedByStudent = table.Column<bool>(type: "bit", nullable: false),
                    IsRejectedByAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsRejectedByTutor = table.Column<bool>(type: "bit", nullable: false),
                    IsRejectedByStudent = table.Column<bool>(type: "bit", nullable: false),
                    Notification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllNotifications_AspNetUsers_ForStudentId",
                        column: x => x.ForStudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_AspNetUsers_ForTutorId",
                        column: x => x.ForTutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_AspNetUsers_NewUserId",
                        column: x => x.NewUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_CourseBookings_CourseBookingId",
                        column: x => x.CourseBookingId,
                        principalTable: "CourseBookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_CourseBookings_CourseBookingIdForLesson",
                        column: x => x.CourseBookingIdForLesson,
                        principalTable: "CourseBookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_LessonBookings_LessonBookingId",
                        column: x => x.LessonBookingId,
                        principalTable: "LessonBookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_LessonBookings_LessonBookingStartId",
                        column: x => x.LessonBookingStartId,
                        principalTable: "LessonBookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_StudentFeedbacks_StudentFeedbackId",
                        column: x => x.StudentFeedbackId,
                        principalTable: "StudentFeedbacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AllNotifications_TutorFeedbacks_TutorFeedbackId",
                        column: x => x.TutorFeedbackId,
                        principalTable: "TutorFeedbacks",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c98e431-4a03-4999-807d-d7ac6ebedac9", "29b705bc-f203-4401-bb9b-b052456a3449", "ApplicationRole", "Tutor", null },
                    { "a2204359-8a88-49c9-867f-a2dd15e3f7c8", "59f8dcf7-b7e8-4731-97c4-fb8652ed0f51", "ApplicationRole", "Guardian", null },
                    { "ad98681d-5abb-4df9-9a9d-4797888220ac", "738ede22-b7d9-475a-bb28-5a5058350a37", "ApplicationRole", "SuperAdmin", null },
                    { "de8dc073-416f-4716-89be-b1c48c9f9de1", "cdfbb1eb-c26c-4662-897a-649775ef8bd3", "ApplicationRole", "Admin", null },
                    { "feb14f16-e7ac-43bf-9bd2-e7de8d1ba71d", "aa48445e-1746-4525-b94d-b26283394355", "ApplicationRole", "Student", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Contract", "CurriculumId", "DateOfBirth", "Discriminator", "Email", "EmailConfirmed", "EmployeeType", "ExtraCaring", "FirstName", "Gender", "GradeId", "HomeAddress", "Image", "IsActive", "IsAdmin", "IsContracttUpdated", "IsDeleted", "IsDocumentUpdated", "IsEducationUpdated", "IsExperienceUpdated", "IsFirstGuardian", "IsFirstGuardianUpdated", "IsProfileUpdated", "IsSecondGuardian", "IsSecondGuardianUpdated", "IsSpecialiitiesUpdated", "IsStudent", "IsSuperAdmin", "IsTutor", "IsUpdated", "LastName", "LocalNumber", "LockoutEnabled", "LockoutEnd", "Nationality", "NormalizedEmail", "NormalizedUserName", "OtherEmployeeType", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileLink", "RegularIrregular", "RelationshipWithStudent", "ResidingCountry", "RoleName", "SchoolName", "SecurityStamp", "StudentId", "Tag", "TimeZone", "TwoFactorEnabled", "Update", "UserName", "WhatsappNumber" },
                values: new object[] { "b9055214-c3b3-4f05-b383-b6e601042b35", 0, "6947db82-43e2-4974-8820-275aa5302715", null, null, null, "ApplicationUser", "superadmin@gmail.com", true, null, false, "SuperAdmin", null, null, null, null, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, null, null, false, null, null, "SUPERADMIN@GMAIL.COM", "SUPERADMIN@GMAIL.COM", null, "AQAAAAEAACcQAAAAEBy+nQwjwzI0Obd1HeJIb/VfZArTILlvoj8Nd0iHd0HY4y8pgav9ZvLVDx4gTcnoiw==", null, false, null, null, null, null, "SuperAdmin", null, "5f175fb0-7da8-42b7-ad22-fdc734ada25a", null, null, null, false, "Not Updated", "superadmin@gmail.com", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { "ad98681d-5abb-4df9-9a9d-4797888220ac", "b9055214-c3b3-4f05-b383-b6e601042b35", "ApplicationUserRole" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminDocuments_ApplicationUserId",
                table: "AdminDocuments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_CourseBookingId",
                table: "AllNotifications",
                column: "CourseBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_CourseBookingIdForLesson",
                table: "AllNotifications",
                column: "CourseBookingIdForLesson");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_ForStudentId",
                table: "AllNotifications",
                column: "ForStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_ForTutorId",
                table: "AllNotifications",
                column: "ForTutorId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_LessonBookingId",
                table: "AllNotifications",
                column: "LessonBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_LessonBookingStartId",
                table: "AllNotifications",
                column: "LessonBookingStartId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_NewUserId",
                table: "AllNotifications",
                column: "NewUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_StudentFeedbackId",
                table: "AllNotifications",
                column: "StudentFeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_AllNotifications_TutorFeedbackId",
                table: "AllNotifications",
                column: "TutorFeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurriculumId",
                table: "AspNetUsers",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GradeId",
                table: "AspNetUsers",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StudentId",
                table: "AspNetUsers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBookings_AdminId",
                table: "CourseBookings",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBookings_CourseId",
                table: "CourseBookings",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBookings_StudentId",
                table: "CourseBookings",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBookings_TutorId",
                table: "CourseBookings",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CurriculumId",
                table: "Courses",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopics_CourseId",
                table: "CourseTopics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeCourses_CourseId",
                table: "GradeCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeCourses_GradeId",
                table: "GradeCourses",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonBookings_CourseBookingId",
                table: "LessonBookings",
                column: "CourseBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCredits_CourseBookingId",
                table: "StudentCredits",
                column: "CourseBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCredits_StudentId",
                table: "StudentCredits",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeedbacks_LessonBookingId",
                table: "StudentFeedbacks",
                column: "LessonBookingId",
                unique: true,
                filter: "[LessonBookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDocuments_TutorId",
                table: "TutorDocuments",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorEducations_TutorId",
                table: "TutorEducations",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorExperiences_CourseId",
                table: "TutorExperiences",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorExperiences_CurriculumId",
                table: "TutorExperiences",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorExperiences_TutorId",
                table: "TutorExperiences",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorFeedbacks_LessonBookingId",
                table: "TutorFeedbacks",
                column: "LessonBookingId",
                unique: true,
                filter: "[LessonBookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSpecialitys_CourseId",
                table: "TutorSpecialitys",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSpecialitys_CurriculumId",
                table: "TutorSpecialitys",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSpecialitys_GradeId",
                table: "TutorSpecialitys",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSpecialitys_TutorId",
                table: "TutorSpecialitys",
                column: "TutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminDocuments");

            migrationBuilder.DropTable(
                name: "AllNotifications");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CourseTopics");

            migrationBuilder.DropTable(
                name: "GradeCourses");

            migrationBuilder.DropTable(
                name: "StudentCredits");

            migrationBuilder.DropTable(
                name: "TutorDocuments");

            migrationBuilder.DropTable(
                name: "TutorEducations");

            migrationBuilder.DropTable(
                name: "TutorExperiences");

            migrationBuilder.DropTable(
                name: "TutorSpecialitys");

            migrationBuilder.DropTable(
                name: "StudentFeedbacks");

            migrationBuilder.DropTable(
                name: "TutorFeedbacks");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "LessonBookings");

            migrationBuilder.DropTable(
                name: "CourseBookings");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Curriculums");
        }
    }
}
