using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Data.Seed;

public static class InterviewSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // ── Create tables if they don't exist ─────────────────────────────
        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InterviewQuestions')
            BEGIN
                CREATE TABLE InterviewQuestions (
                    Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
                    Question    NVARCHAR(1000)   NOT NULL,
                    ModelAnswer NVARCHAR(MAX)    NOT NULL,
                    Category    NVARCHAR(100)    NOT NULL,
                    Difficulty  NVARCHAR(50)     NOT NULL,
                    Tags        NVARCHAR(300)    NOT NULL DEFAULT '',
                    OrderNumber INT              NOT NULL DEFAULT 0,
                    CONSTRAINT PK_InterviewQuestions PRIMARY KEY (Id)
                );
                CREATE INDEX IX_InterviewQuestions_Category ON InterviewQuestions(Category);
            END");

        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserInterviewPractices')
            BEGIN
                CREATE TABLE UserInterviewPractices (
                    Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
                    UserId      UNIQUEIDENTIFIER NOT NULL,
                    QuestionId  UNIQUEIDENTIFIER NOT NULL,
                    SelfRating  INT              NOT NULL DEFAULT 0,
                    Notes       NVARCHAR(2000)   NULL,
                    PracticedAt DATETIME2        NOT NULL,
                    CONSTRAINT PK_UserInterviewPractices PRIMARY KEY (Id),
                    CONSTRAINT UQ_UserInterviewPractices UNIQUE (UserId, QuestionId),
                    CONSTRAINT FK_UserInterviewPractices_Questions FOREIGN KEY (QuestionId)
                        REFERENCES InterviewQuestions(Id) ON DELETE CASCADE,
                    CONSTRAINT FK_UserInterviewPractices_Users FOREIGN KEY (UserId)
                        REFERENCES AspNetUsers(Id) ON DELETE CASCADE
                );
                CREATE INDEX IX_UserInterviewPractices_QuestionId ON UserInterviewPractices(QuestionId);
            END");

        // ── Skip if questions already seeded ─────────────────────────────
        if (await context.InterviewQuestions.AnyAsync()) return;

        // ── Question definitions ──────────────────────────────────────────
        var questions = new List<(string Q, string A, string Cat, string Diff, string Tags, int Order)>
        {
            // ════════════════════════════════════════════════════════
            // PYTHON
            // ════════════════════════════════════════════════════════
            (
                "What is the difference between a list and a tuple in Python?",
                "Lists are mutable — you can add, remove, or change elements after creation. Tuples are immutable — once created, their contents cannot be changed.\n\n• Syntax: lists use [], tuples use ()\n• Performance: tuples are slightly faster and use less memory because they are fixed-size\n• Use cases: use lists for collections that change (e.g., a shopping cart); use tuples for fixed data (e.g., coordinates, RGB values, database records)\n• Hashability: tuples can be used as dictionary keys or set elements (if all their contents are hashable); lists cannot\n\nExample:\nmy_list = [1, 2, 3]    # mutable\nmy_tuple = (1, 2, 3)   # immutable",
                "Python", "Beginner", "data-structures,basics", 1
            ),
            (
                "What are Python decorators and how do they work?",
                "A decorator is a function that wraps another function to extend or modify its behaviour without changing its source code. Decorators use Python's first-class function support.\n\nHow they work:\n1. A decorator takes a function as an argument\n2. Defines a wrapper function inside\n3. Returns the wrapper\n\nExample:\ndef log(func):\n    def wrapper(*args, **kwargs):\n        print(f'Calling {func.__name__}')\n        return func(*args, **kwargs)\n    return wrapper\n\n@log\ndef greet(name):\n    print(f'Hello {name}')\n\ngreet('Alice')  # prints 'Calling greet' then 'Hello Alice'\n\nCommon built-in decorators: @staticmethod, @classmethod, @property, @functools.wraps",
                "Python", "Intermediate", "decorators,functions,closures", 2
            ),
            (
                "Explain Python's *args and **kwargs.",
                "*args allows a function to accept any number of positional arguments. Inside the function, args is a tuple.\n\n**kwargs allows a function to accept any number of keyword arguments. Inside the function, kwargs is a dictionary.\n\nExample:\ndef demo(*args, **kwargs):\n    for arg in args:\n        print(arg)\n    for key, val in kwargs.items():\n        print(f'{key} = {val}')\n\ndemo(1, 2, 3, name='Alice', age=30)\n\nUse cases:\n• Building flexible APIs (e.g., print() accepts *objects)\n• Forwarding arguments: def child(*args, **kwargs): parent(*args, **kwargs)\n• Decorators that need to pass all arguments through",
                "Python", "Beginner", "functions,args,kwargs", 3
            ),
            (
                "What is the Global Interpreter Lock (GIL) in Python?",
                "The GIL is a mutex in CPython (the standard Python implementation) that allows only one thread to hold control of the Python interpreter at a time.\n\nWhy it exists: CPython's memory management (reference counting) is not thread-safe. The GIL prevents multiple threads from corrupting objects simultaneously.\n\nImpact:\n• CPU-bound programs: multiple threads don't truly run in parallel — the GIL creates a bottleneck\n• I/O-bound programs: threads release the GIL during I/O waits, so threading still helps\n\nWorkarounds:\n• Use multiprocessing instead of threading for CPU-bound work (each process has its own GIL)\n• Use Jython or PyPy-STM which don't have a GIL\n• Use C extensions (NumPy) that release the GIL internally",
                "Python", "Advanced", "threading,concurrency,cpython", 4
            ),
            (
                "What are list comprehensions and how do they differ from generator expressions?",
                "List comprehension creates an entire list in memory at once:\nsquares = [x**2 for x in range(10)]\n\nGenerator expression produces values one at a time (lazy evaluation):\nsquares_gen = (x**2 for x in range(10))\n\nKey differences:\n• Memory: list comprehension stores all values; generator stores nothing — it yields on demand\n• Speed: generators are faster to create; lists are faster to iterate repeatedly\n• Syntax: [] vs ()\n• Reusability: a list can be iterated multiple times; a generator is exhausted after one pass\n\nWhen to use generators:\n• Working with large datasets that don't need to fit in memory\n• Pipelines where you only need one value at a time\n• sum(x**2 for x in range(1_000_000)) — no list created",
                "Python", "Intermediate", "comprehensions,generators,memory", 5
            ),
            (
                "How does Python manage memory?",
                "Python uses automatic memory management with three main mechanisms:\n\n1. Reference Counting — every object has a counter of how many references point to it. When the count reaches 0, the object is immediately deallocated.\n\n2. Garbage Collector — handles reference cycles (A → B → A) that reference counting alone cannot free. The gc module runs periodically.\n\n3. Memory Pools — CPython uses a private heap and pools small objects (< 512 bytes) in arenas to reduce OS-level allocations and fragmentation.\n\nKey tools:\n• sys.getrefcount(obj) — view reference count\n• gc.collect() — force garbage collection\n• tracemalloc — trace memory allocations\n\nNote: del x doesn't free memory; it just removes the name binding. Memory is freed when the reference count drops to 0.",
                "Python", "Advanced", "memory,gc,reference-counting", 6
            ),
            (
                "What is the difference between __str__ and __repr__?",
                "__str__ is meant to return a human-readable string representation — it's what print() and str() use. Aim for readability.\n\n__repr__ is meant to return an unambiguous, developer-oriented representation — ideally one that could recreate the object. It's what the REPL uses and what repr() calls.\n\nExample:\nclass Point:\n    def __init__(self, x, y):\n        self.x, self.y = x, y\n    def __repr__(self):\n        return f'Point({self.x}, {self.y})'\n    def __str__(self):\n        return f'({self.x}, {self.y})'\n\np = Point(3, 4)\nprint(str(p))   # (3, 4)\nprint(repr(p))  # Point(3, 4)\n\nRule of thumb: always implement __repr__. If __str__ is missing, Python falls back to __repr__.",
                "Python", "Intermediate", "dunder,oop,string-representation", 7
            ),
            (
                "What are context managers and how do you create one?",
                "A context manager defines setup and teardown logic that runs around a block of code using the 'with' statement. The most common use is ensuring resources (files, locks, DB connections) are properly released.\n\nTwo ways to create one:\n\n1. Class-based (implement __enter__ and __exit__):\nclass Timer:\n    def __enter__(self):\n        self.start = time.time()\n        return self\n    def __exit__(self, *args):\n        print(f'Elapsed: {time.time() - self.start:.2f}s')\n\nwith Timer() as t:\n    time.sleep(1)\n\n2. Generator-based (using contextlib.contextmanager):\nfrom contextlib import contextmanager\n\n@contextmanager\ndef managed_file(name):\n    f = open(name, 'w')\n    try:\n        yield f\n    finally:\n        f.close()\n\nwith managed_file('test.txt') as f:\n    f.write('hello')",
                "Python", "Advanced", "context-manager,with,resources", 8
            ),

            // ════════════════════════════════════════════════════════
            // JAVA
            // ════════════════════════════════════════════════════════
            (
                "What is the difference between JDK, JRE, and JVM?",
                "JVM (Java Virtual Machine): the runtime engine that executes Java bytecode. It's platform-specific but provides a platform-independent execution environment for .class files. Handles memory management, garbage collection, and bytecode interpretation/JIT compilation.\n\nJRE (Java Runtime Environment): JVM + standard class libraries (java.lang, java.util, etc.). Everything needed to run a Java program — nothing more.\n\nJDK (Java Development Kit): JRE + development tools (javac compiler, javadoc, jar, debugger, etc.). Everything needed to develop AND run Java programs.\n\nMemory model: JDK ⊃ JRE ⊃ JVM\n\nQuick rule: end-users need JRE; developers need JDK.",
                "Java", "Beginner", "jvm,jre,jdk,platform", 1
            ),
            (
                "What is the difference between == and .equals() in Java?",
                "== compares references — it checks whether two variables point to the exact same object in memory.\n\n.equals() compares content — it checks whether two objects are logically equal, based on their equals() implementation.\n\nExample:\nString a = new String(\"hello\");\nString b = new String(\"hello\");\n\nSystem.out.println(a == b);        // false — different objects\nSystem.out.println(a.equals(b));   // true — same content\n\nString literals are interned:\nString c = \"hello\";\nString d = \"hello\";\nSystem.out.println(c == d);  // true — same interned object\n\nImportant: for primitives (int, char, etc.), == always compares values because primitives are not objects. Always use .equals() for objects, especially Strings.",
                "Java", "Beginner", "equality,strings,references", 2
            ),
            (
                "What is the difference between an interface and an abstract class in Java?",
                "Abstract class:\n• Can have both abstract methods (no body) and concrete methods (with body)\n• Can have instance variables, constructors, and any access modifiers\n• A class can extend only ONE abstract class (single inheritance)\n• Use when classes share a common base implementation\n\nInterface:\n• Historically: only abstract methods. Since Java 8: default and static methods allowed. Since Java 9: private methods allowed\n• Variables are implicitly public static final (constants)\n• A class can implement MULTIPLE interfaces\n• Use when you want to define a contract (capability) regardless of class hierarchy\n\nWhen to use which:\n• Abstract class: IS-A relationship with shared state/behaviour (e.g., Vehicle → Car)\n• Interface: HAS-A capability across unrelated classes (e.g., Serializable, Runnable, Comparable)",
                "Java", "Intermediate", "oop,interface,abstract,inheritance", 3
            ),
            (
                "Explain Java's garbage collection.",
                "Java's GC automatically reclaims memory occupied by objects that are no longer reachable from any live reference, preventing memory leaks without manual deallocation.\n\nHow it works:\n1. Objects are allocated in the Heap, divided into Young Generation (Eden + Survivor spaces) and Old Generation\n2. Minor GC: runs frequently, collects short-lived objects from Young Gen using a copying algorithm\n3. Major/Full GC: runs less often, collects Old Gen using mark-sweep-compact\n4. Objects surviving multiple Minor GCs are promoted to Old Gen\n\nCommon GC algorithms:\n• Serial GC — single thread, simple\n• Parallel GC — multi-threaded throughput collector\n• G1 GC (default since Java 9) — splits heap into regions, low-pause\n• ZGC / Shenandoah — sub-millisecond pause times\n\nTuning: -Xmx (max heap), -Xms (initial heap), -XX:+UseG1GC",
                "Java", "Advanced", "gc,memory,heap,jvm", 4
            ),
            (
                "What are Java Streams and when would you use them?",
                "Streams (java.util.stream) provide a declarative, functional-style API for processing sequences of elements — filtering, mapping, reducing — without writing explicit loops.\n\nKey characteristics:\n• Lazy: intermediate operations (filter, map) are not executed until a terminal operation (collect, forEach, count) is called\n• Not a data structure — they don't store elements\n• Can be sequential or parallel\n\nExample:\nList<String> names = List.of(\"Alice\", \"Bob\", \"Charlie\", \"Anna\");\n\nList<String> result = names.stream()\n    .filter(n -> n.startsWith(\"A\"))  // intermediate\n    .map(String::toUpperCase)         // intermediate\n    .sorted()                         // intermediate\n    .collect(Collectors.toList());    // terminal\n\n// result: [\"ALICE\", \"ANNA\"]\n\nWhen to use: replacing verbose for-loops, data transformation pipelines, parallel processing of large collections with .parallelStream()",
                "Java", "Intermediate", "streams,functional,lambda,collections", 5
            ),
            (
                "What is the difference between checked and unchecked exceptions in Java?",
                "Checked exceptions: the compiler forces you to handle them (try/catch) or declare them with throws. They represent recoverable conditions outside the program's control.\nExamples: IOException, SQLException, FileNotFoundException\n\nUnchecked exceptions (RuntimeException subclasses): the compiler does NOT require handling. They represent programming errors.\nExamples: NullPointerException, ArrayIndexOutOfBoundsException, IllegalArgumentException\n\nError: a separate hierarchy (e.g., OutOfMemoryError, StackOverflowError) — critical JVM failures, not meant to be caught.\n\nBest practice:\n• Use checked exceptions for recoverable errors the caller should know about\n• Use unchecked exceptions for bugs (invalid arguments, violated invariants)\n• Avoid catching Exception or Throwable broadly — it hides bugs",
                "Java", "Intermediate", "exceptions,error-handling,checked,unchecked", 6
            ),
            (
                "What is autoboxing and unboxing in Java?",
                "Java has both primitive types (int, double, boolean, etc.) and their corresponding wrapper classes (Integer, Double, Boolean, etc.).\n\nAutoboxing: automatic conversion from a primitive to its wrapper class.\nUnboxing: automatic conversion from a wrapper class back to its primitive.\n\nExample:\nInteger boxed = 42;        // autoboxing — compiler converts: Integer.valueOf(42)\nint primitive = boxed;     // unboxing — compiler converts: boxed.intValue()\n\nList<Integer> list = new ArrayList<>();\nlist.add(5);   // autoboxing — primitives can't be stored in generics directly\nint x = list.get(0);  // unboxing\n\nPitfalls:\n• NullPointerException: Integer n = null; int x = n; // NPE on unboxing!\n• Performance: in tight loops, autoboxing creates many temporary objects\n• Integer caching: Integer.valueOf(-128 to 127) returns cached instances, so == may unexpectedly return true",
                "Java", "Intermediate", "autoboxing,primitives,wrappers,generics", 7
            ),
            (
                "What is the volatile keyword in Java?",
                "volatile guarantees that reads and writes to a variable are always done directly from/to main memory, not from a thread's local CPU cache.\n\nWithout volatile: each thread may cache a variable's value. Thread A writes a new value, but Thread B still reads the stale cached value.\n\nWith volatile: all threads always see the most recently written value.\n\nExample:\nclass StopFlag {\n    private volatile boolean stopped = false;\n\n    public void stop() { stopped = true; }\n    public void run() {\n        while (!stopped) { /* work */ }\n    }\n}\n\nWhen to use volatile:\n• Simple flags read by multiple threads\n• When only one thread writes, multiple threads read\n\nWhen NOT to use (use synchronized or AtomicInteger instead):\n• When you need compound operations (check-then-act, increment i++) — these are not atomic even with volatile",
                "Java", "Advanced", "concurrency,threading,volatile,memory-model", 8
            ),

            // ════════════════════════════════════════════════════════
            // WEB DEVELOPMENT
            // ════════════════════════════════════════════════════════
            (
                "What is the difference between == and === in JavaScript?",
                "== (loose equality) performs type coercion before comparing — it converts both values to the same type if they differ.\n\n=== (strict equality) compares both value AND type without any conversion.\n\nExamples:\n0 == false    // true  (false coerces to 0)\n0 === false   // false (different types)\n'' == false   // true  (both coerce to 0)\n'' === false  // false\nnull == undefined  // true\nnull === undefined // false\n1 == '1'     // true  ('1' coerces to 1)\n1 === '1'    // false\n\nBest practice: always use === unless you have a specific reason for type coercion. Using === prevents subtle bugs and makes intent explicit.",
                "Web Development", "Beginner", "javascript,equality,types,coercion", 1
            ),
            (
                "What is event delegation in JavaScript?",
                "Event delegation is a pattern where you attach a single event listener to a parent element instead of multiple listeners to individual child elements. It works because events bubble up through the DOM.\n\nWhy use it:\n• Performance: one listener instead of hundreds (e.g., a list with 1000 items)\n• Dynamic content: works for elements added to the DOM after the listener is attached\n\nExample without delegation (bad for large lists):\ndocument.querySelectorAll('li').forEach(li =>\n  li.addEventListener('click', handleClick)\n);\n\nWith delegation:\ndocument.getElementById('list').addEventListener('click', (e) => {\n  if (e.target.tagName === 'LI') {\n    handleClick(e.target);\n  }\n});\n\nKey concept: e.target is the actual element clicked; e.currentTarget is the element the listener is attached to.",
                "Web Development", "Intermediate", "javascript,events,dom,performance", 2
            ),
            (
                "What is the difference between let, const, and var in JavaScript?",
                "var:\n• Function-scoped (or globally scoped)\n• Hoisted to the top of its scope (initialised as undefined)\n• Can be re-declared and re-assigned\n• Avoid in modern code — it causes confusing behaviour\n\nlet:\n• Block-scoped (lives within the nearest {} block)\n• Hoisted but NOT initialised (Temporal Dead Zone — accessing before declaration throws ReferenceError)\n• Can be re-assigned but NOT re-declared in the same scope\n\nconst:\n• Block-scoped, same as let\n• Must be initialised at declaration\n• Cannot be re-assigned (the binding is constant)\n• Important: objects/arrays declared with const can still have their contents mutated — const only prevents reassignment of the variable itself\n\nBest practice: default to const; use let when you need to reassign; never use var.",
                "Web Development", "Beginner", "javascript,variables,scope,hoisting", 3
            ),
            (
                "What is a Promise in JavaScript and how does it work?",
                "A Promise is an object representing the eventual completion or failure of an asynchronous operation. It has three states: pending, fulfilled, or rejected.\n\nCreating a Promise:\nconst promise = new Promise((resolve, reject) => {\n  setTimeout(() => resolve('done!'), 1000);\n});\n\nConsuming a Promise:\npromise\n  .then(result => console.log(result))  // 'done!'\n  .catch(err => console.error(err))\n  .finally(() => console.log('cleanup'));\n\nPromise chaining: each .then() returns a new Promise, enabling sequential async steps.\n\nasync/await is syntactic sugar over Promises:\nasync function fetchData() {\n  try {\n    const data = await fetchFromServer();\n    console.log(data);\n  } catch (err) {\n    console.error(err);\n  }\n}\n\nPromise utilities: Promise.all (all must resolve), Promise.race (first to settle wins), Promise.allSettled, Promise.any",
                "Web Development", "Intermediate", "javascript,async,promises,callbacks", 4
            ),
            (
                "What is CORS and how do you handle it?",
                "CORS (Cross-Origin Resource Sharing) is a browser security mechanism that restricts web pages from making requests to a different origin (domain, port, or protocol) than the one that served the page.\n\nWhy it exists: prevents malicious sites from making authenticated requests to other sites on behalf of a logged-in user (CSRF attacks).\n\nHow it works:\n• Simple requests: browser adds Origin header; server must respond with Access-Control-Allow-Origin header\n• Preflight requests: for complex requests (non-GET/POST, custom headers), browser sends an OPTIONS request first\n\nHandling CORS on the server:\n// Express.js example\napp.use((req, res, next) => {\n  res.setHeader('Access-Control-Allow-Origin', 'https://myapp.com');\n  res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');\n  res.setHeader('Access-Control-Allow-Headers', 'Content-Type, Authorization');\n  next();\n});\n\nCommon mistake: 'Access-Control-Allow-Origin: *' + credentials — this is not allowed; you must specify an exact origin when sending cookies.",
                "Web Development", "Intermediate", "http,security,cors,api", 5
            ),
            (
                "Explain CSS specificity.",
                "CSS specificity determines which style rule wins when multiple rules target the same element. It's calculated as a score with four components:\n\n(inline, IDs, classes/attributes/pseudo-classes, elements/pseudo-elements)\n\nSpecificity values:\n• Element selector: (0,0,0,1) — e.g., p, div\n• Class, attribute, pseudo-class: (0,0,1,0) — e.g., .btn, [type], :hover\n• ID selector: (0,1,0,0) — e.g., #header\n• Inline style: (1,0,0,0) — e.g., style='color:red'\n• !important: overrides everything (avoid when possible)\n\nComparison:\n#nav .link:hover  → (0,1,1,1)\ndiv p.text        → (0,0,1,2)\n\nWhen specificity is equal, the rule that appears later in the CSS wins.\n\nBest practice: keep specificity low and consistent. Prefer classes over IDs for styling. Avoid !important — it breaks the cascade and makes debugging painful.",
                "Web Development", "Intermediate", "css,specificity,cascade,selectors", 6
            ),
            (
                "What is the difference between null and undefined in JavaScript?",
                "undefined: a variable has been declared but not yet assigned a value. It's the default initial value. Also returned when accessing non-existent object properties or function parameters that weren't passed.\n\nnull: an intentional assignment meaning 'no value'. It must be explicitly set by the developer.\n\nExamples:\nlet x;               // x is undefined\nlet y = null;        // y is explicitly 'no value'\n\nfunction greet(name) {\n  console.log(name); // undefined if no argument passed\n}\n\ntypeof undefined  // 'undefined'\ntypeof null       // 'object' — famous JavaScript bug, kept for compatibility\n\nnull == undefined   // true (loose equality)\nnull === undefined  // false (strict equality)\n\nBest practice: use null to represent intentional absence of a value. Use optional chaining (?.) to safely access potentially null/undefined properties: user?.address?.city",
                "Web Development", "Beginner", "javascript,null,undefined,types", 7
            ),
            (
                "What are the principles of RESTful API design?",
                "REST (Representational State Transfer) is an architectural style for designing networked applications. The 6 key constraints:\n\n1. Stateless: each request must contain all information needed to process it. Server stores no client state between requests.\n\n2. Client-Server: separation of concerns — client handles UI, server handles data/logic.\n\n3. Uniform Interface:\n   • Resources identified by URIs: GET /users/42\n   • HTTP methods convey intent: GET (read), POST (create), PUT/PATCH (update), DELETE (remove)\n   • Self-descriptive messages (Content-Type headers)\n\n4. Cacheable: responses should indicate whether they can be cached.\n\n5. Layered System: client doesn't need to know if it's talking directly to the server or through a proxy/load balancer.\n\n6. Code on Demand (optional): server can send executable code (e.g., JavaScript) to the client.\n\nBest practices:\n• Use nouns for endpoints, not verbs: /users not /getUsers\n• Use HTTP status codes correctly: 200 OK, 201 Created, 400 Bad Request, 404 Not Found, 500 Internal Server Error\n• Version your API: /api/v1/users",
                "Web Development", "Intermediate", "api,rest,http,design", 8
            ),

            // ════════════════════════════════════════════════════════
            // GENERAL CS
            // ════════════════════════════════════════════════════════
            (
                "What is the difference between a stack and a queue?",
                "Stack — Last In, First Out (LIFO):\n• Elements are added (push) and removed (pop) from the SAME end (the 'top')\n• Like a stack of plates — you take from the top\n• Operations: push, pop, peek — all O(1)\n• Use cases: function call stack, undo/redo, parsing expressions, backtracking algorithms, DFS\n\nQueue — First In, First Out (FIFO):\n• Elements are added (enqueue) at the back and removed (dequeue) from the front\n• Like a checkout line — first in, first served\n• Operations: enqueue, dequeue, peek — all O(1)\n• Use cases: BFS, task scheduling, print queues, message queues, request handling\n\nVariants:\n• Deque (double-ended queue): insert/remove from both ends\n• Priority Queue: dequeue in priority order, not insertion order",
                "General CS", "Beginner", "data-structures,stack,queue,lifo,fifo", 1
            ),
            (
                "Explain Big O notation and why it matters.",
                "Big O notation describes the worst-case time or space complexity of an algorithm as input size n grows toward infinity. It tells us how performance scales, ignoring constants and lower-order terms.\n\nCommon complexities (best to worst):\n• O(1) — constant: array access by index\n• O(log n) — logarithmic: binary search, balanced BST operations\n• O(n) — linear: single loop, linear search\n• O(n log n) — linearithmic: merge sort, heap sort, efficient comparison sorting\n• O(n²) — quadratic: nested loops, bubble sort, insertion sort on unsorted data\n• O(2ⁿ) — exponential: recursive Fibonacci without memoisation\n• O(n!) — factorial: permutations, brute-force TSP\n\nWhy it matters:\n• An O(n²) algorithm on 1 million elements takes 10¹² operations — unusable\n• An O(n log n) algorithm on the same data takes ~20 million — fine\n• Big O guides algorithm selection for production systems",
                "General CS", "Beginner", "algorithms,complexity,big-o,performance", 2
            ),
            (
                "What is a hash table and how does it work?",
                "A hash table (hash map) stores key-value pairs and provides average O(1) time for insert, lookup, and delete.\n\nHow it works:\n1. A hash function converts the key to an integer index\n2. The value is stored at that index in an underlying array\n3. On lookup, the same hash function finds the index\n\nCollisions (two keys hash to the same index):\n• Chaining: each slot holds a linked list of entries — O(1) average, O(n) worst case\n• Open addressing: probe for the next empty slot (linear probing, quadratic probing, double hashing)\n\nLoad factor = number of entries / number of slots. When it exceeds a threshold (e.g., 0.75), the table is resized (rehashed).\n\nReal-world examples:\n• Python dict, set\n• Java HashMap, HashSet\n• JavaScript plain objects and Map\n\nPitfall: mutable objects as keys — if the key changes after insertion, the hash changes and the entry becomes unreachable.",
                "General CS", "Intermediate", "data-structures,hash-table,hashing,collision", 3
            ),
            (
                "What is recursion and when should you use it?",
                "Recursion is when a function calls itself to solve a smaller version of the same problem, until a base case is reached.\n\nStructure of every recursive function:\n1. Base case: condition that stops recursion (prevents infinite loop)\n2. Recursive case: call self with a smaller/simpler input\n\nExample — factorial:\ndef factorial(n):\n    if n <= 1: return 1        # base case\n    return n * factorial(n-1)  # recursive case\n\nWhen to use recursion:\n• Problems with naturally recursive structure: tree traversal, graph DFS, parsing nested structures, divide-and-conquer (merge sort, quicksort)\n• When the recursive solution is significantly clearer than the iterative one\n\nWhen NOT to use:\n• When n is large and there's no tail-call optimisation (risk of stack overflow)\n• When iteration is just as clear and more efficient\n\nTrade-offs: recursion uses the call stack (O(depth) space); iteration uses O(1) space. Memoisation converts exponential recursion to polynomial time.",
                "General CS", "Intermediate", "algorithms,recursion,stack,divide-conquer", 4
            ),
            (
                "Explain the SOLID principles.",
                "SOLID is five object-oriented design principles that make software easier to maintain and extend:\n\nS — Single Responsibility Principle: a class should have only one reason to change. Don't mix business logic with persistence logic in one class.\n\nO — Open/Closed Principle: software entities should be open for extension but closed for modification. Add behaviour by adding new code (subclasses, decorators), not by changing existing code.\n\nL — Liskov Substitution Principle: subclasses must be substitutable for their base class without breaking the program. If Square extends Rectangle, replacing a Rectangle with a Square must not cause bugs.\n\nI — Interface Segregation Principle: clients should not be forced to depend on interfaces they don't use. Split large interfaces into smaller, focused ones.\n\nD — Dependency Inversion Principle: high-level modules should not depend on low-level modules — both should depend on abstractions (interfaces). Inject dependencies rather than hard-coding them.\n\nSummary: these principles reduce coupling, increase cohesion, and make code testable.",
                "General CS", "Intermediate", "oop,design-principles,solid,architecture", 5
            ),
            (
                "What is a deadlock and how do you prevent it?",
                "A deadlock occurs when two or more threads are permanently blocked, each waiting for a resource held by the other.\n\nExample:\nThread A holds Lock 1, waits for Lock 2\nThread B holds Lock 2, waits for Lock 1\n→ Neither can proceed\n\nFour necessary conditions (Coffman conditions) — ALL must hold for deadlock:\n1. Mutual Exclusion: resources can't be shared\n2. Hold and Wait: a thread holds a resource while waiting for another\n3. No Preemption: resources can't be forcibly taken\n4. Circular Wait: circular chain of threads each waiting on the next\n\nPrevention strategies:\n• Lock ordering: always acquire locks in a fixed global order (eliminates circular wait)\n• Lock timeout: use tryLock(timeout) instead of lock() — give up if can't acquire\n• Deadlock detection: detect cycles in the wait-for graph and break them\n• Avoid nested locks: design so you never hold one lock while acquiring another",
                "General CS", "Advanced", "concurrency,deadlock,threading,synchronisation", 6
            ),
            (
                "What is the difference between a process and a thread?",
                "Process:\n• An independent program in execution with its own memory space (heap, stack, code, data segments)\n• Isolated from other processes — one process crashing doesn't affect others\n• Communication via IPC (pipes, sockets, shared memory) — more overhead\n• Spawning is expensive (fork() clones the entire memory space)\n\nThread:\n• A unit of execution within a process — multiple threads share the same memory space\n• Lightweight compared to processes — creation and context switching are cheaper\n• Communication via shared memory — fast, but requires synchronisation (locks, mutexes)\n• One thread crashing can bring down the entire process\n\nUse processes when:\n• Isolation is critical (e.g., browser tabs as separate processes)\n• Running on multiple machines\n\nUse threads when:\n• Shared data access is needed\n• I/O-bound work that can proceed concurrently\n\nPython note: threads are limited by the GIL for CPU-bound tasks — use multiprocessing instead.",
                "General CS", "Intermediate", "os,process,thread,concurrency", 7
            ),
            (
                "What is polymorphism and what are its types?",
                "Polymorphism means 'many forms' — it allows objects of different types to be treated through a common interface, with behaviour determined at runtime or compile time.\n\n1. Compile-time (static) polymorphism — Method Overloading:\n   Same method name, different parameters. Resolved at compile time.\n   void print(int x) { ... }\n   void print(String s) { ... }\n\n2. Runtime (dynamic) polymorphism — Method Overriding:\n   Subclass provides a different implementation of a superclass method. Resolved at runtime via virtual dispatch.\n   class Animal { void speak() { print('...'); } }\n   class Dog extends Animal { void speak() { print('Woof'); } }\n   Animal a = new Dog(); a.speak(); // prints 'Woof'\n\n3. Parametric polymorphism — Generics:\n   Code that works with any type: List<T>, Optional<T>\n\n4. Duck typing (Python/JavaScript):\n   'If it walks like a duck and quacks like a duck, it's a duck' — no explicit interface needed; any object with the right methods works.\n\nWhy it matters: polymorphism enables code to work with future types without modification (Open/Closed Principle).",
                "General CS", "Intermediate", "oop,polymorphism,overriding,overloading", 8
            ),

            // ════════════════════════════════════════════════════════
            // ALGORITHMS
            // ════════════════════════════════════════════════════════
            (
                "How would you find two numbers in an array that sum to a target?",
                "Brute force: two nested loops checking every pair — O(n²) time, O(1) space.\n\nOptimal approach using a hash set — O(n) time, O(n) space:\n\ndef two_sum(nums, target):\n    seen = {}                          # value → index\n    for i, num in enumerate(nums):\n        complement = target - num\n        if complement in seen:\n            return [seen[complement], i]\n        seen[num] = i\n    return []\n\nWhy it works:\nFor each number, we check if its complement (target - num) is already in the hash map. If yes, we found our pair. If no, we add the current number to the map.\n\nExample: nums=[2,7,11,15], target=9\n• i=0: complement=7, not in seen; seen={2:0}\n• i=1: complement=2, found at index 0 → return [0,1]\n\nEdge cases to handle:\n• Duplicate numbers: e.g., [3,3] with target=6 — works if we check before inserting\n• No solution: return [] or raise exception\n• Negative numbers: works the same way",
                "Algorithms", "Beginner", "arrays,hash-map,two-pointers,searching", 1
            ),
            (
                "Explain the difference between bubble sort and quicksort.",
                "Bubble Sort:\n• Algorithm: repeatedly swap adjacent elements if they are in the wrong order; after each pass, the largest unsorted element 'bubbles' to the end\n• Time: O(n²) average and worst case, O(n) best case (already sorted with optimisation)\n• Space: O(1) — in-place\n• Stable: yes\n• Practical use: almost never in production — mainly for teaching. Tiny advantage: simple to implement and O(n) on nearly-sorted data\n\nQuicksort:\n• Algorithm: pick a pivot, partition array so all smaller elements are left and larger are right, recursively sort each partition\n• Time: O(n log n) average, O(n²) worst case (bad pivot selection — e.g., always picking smallest/largest on sorted input)\n• Space: O(log n) average due to recursion stack\n• Stable: no (standard implementation)\n• Practical use: widely used in practice — most standard library sorts (e.g., Java Arrays.sort for primitives, C++ std::sort use introsort/timsort variants)\n\nKey insight: quicksort is fast in practice because of excellent cache behaviour and low constant factors, despite theoretical O(n²) worst case.",
                "Algorithms", "Intermediate", "sorting,bubble-sort,quicksort,complexity", 2
            ),
            (
                "What is dynamic programming and when do you use it?",
                "Dynamic programming (DP) is an optimisation technique that breaks a problem into overlapping subproblems, solves each subproblem once, and stores the results (memoisation or tabulation) to avoid redundant computation.\n\nTwo approaches:\n1. Top-down (memoisation): start with the original problem, recurse, cache results\n2. Bottom-up (tabulation): solve smallest subproblems first, build up to the solution\n\nWhen to use DP:\n• Problem has optimal substructure: optimal solution can be built from optimal solutions of subproblems\n• Problem has overlapping subproblems: same subproblems are solved multiple times in the recursive tree\n\nExample — Fibonacci with memoisation:\nmemo = {}\ndef fib(n):\n    if n <= 1: return n\n    if n in memo: return memo[n]\n    memo[n] = fib(n-1) + fib(n-2)\n    return memo[n]\n# O(n) instead of O(2^n)\n\nClassic DP problems: Fibonacci, Knapsack, Longest Common Subsequence, Coin Change, Shortest Path (Bellman-Ford)",
                "Algorithms", "Advanced", "dynamic-programming,memoisation,optimisation", 3
            ),
            (
                "How do you detect a cycle in a linked list?",
                "Floyd's Cycle Detection Algorithm (tortoise and hare) — O(n) time, O(1) space:\n\nUse two pointers:\n• slow: advances one step at a time\n• fast: advances two steps at a time\n\nIf there is a cycle, fast will eventually catch up to slow inside the cycle. If there is no cycle, fast will reach the end (null).\n\ndef has_cycle(head):\n    slow, fast = head, head\n    while fast and fast.next:\n        slow = slow.next\n        fast = fast.next.next\n        if slow is fast:\n            return True\n    return False\n\nFinding the start of the cycle (optional):\nAfter detection, move one pointer back to head. Both pointers advance one step at a time. They meet at the cycle start.\n\nAlternative (uses O(n) space): store visited nodes in a hash set. If you visit the same node twice, there's a cycle.\n\nWhy Floyd's is preferred: constant extra space, elegant mathematics.",
                "Algorithms", "Intermediate", "linked-list,cycle-detection,two-pointers", 4
            ),
            (
                "Explain BFS vs DFS and when to use each.",
                "BFS (Breadth-First Search):\n• Explores level by level using a queue\n• Visits all nodes at distance k before any at distance k+1\n• Time/Space: O(V + E) where V=vertices, E=edges\n• Memory: O(width of graph) — can be large for wide graphs\n\nUse BFS when:\n• Finding the shortest path (unweighted graphs)\n• Level-order traversal of a tree\n• Finding all nodes within k hops\n\nDFS (Depth-First Search):\n• Explores as deep as possible before backtracking using a stack (or recursion)\n• Time: O(V + E), Space: O(depth of graph)\n• Memory: O(depth) — better for deep graphs\n\nUse DFS when:\n• Detecting cycles\n• Topological sorting\n• Solving mazes, puzzles (backtracking)\n• Finding connected components\n• Tree operations (pre/in/post-order traversal)\n\nKey difference: BFS guarantees shortest path in unweighted graphs. DFS does not, but uses less memory on wide graphs.",
                "Algorithms", "Intermediate", "graphs,bfs,dfs,traversal,shortest-path", 5
            ),
            (
                "How would you check if a string is a palindrome?",
                "A palindrome reads the same forwards and backwards (e.g., 'racecar', 'A man a plan a canal Panama').\n\nSimplest approach (two-pointer) — O(n) time, O(1) space:\n\ndef is_palindrome(s):\n    # Normalise: lowercase, remove non-alphanumeric\n    s = ''.join(c.lower() for c in s if c.isalnum())\n    left, right = 0, len(s) - 1\n    while left < right:\n        if s[left] != s[right]:\n            return False\n        left += 1\n        right -= 1\n    return True\n\nAlternative (reverse) — O(n) time, O(n) space:\ns_clean = normalise(s)\nreturn s_clean == s_clean[::-1]\n\nFor numbers:\ndef is_palindrome_num(n):\n    if n < 0: return False\n    s = str(n)\n    return s == s[::-1]\n\nEdge cases:\n• Empty string → True\n• Single character → True\n• Numbers: negative numbers → False\n• Case sensitivity and special characters (usually ignored per problem spec)",
                "Algorithms", "Beginner", "strings,two-pointers,palindrome", 6
            ),
            (
                "What is binary search and when can you use it?",
                "Binary search finds a target in a sorted array in O(log n) time by repeatedly halving the search space.\n\nAlgorithm:\ndef binary_search(arr, target):\n    left, right = 0, len(arr) - 1\n    while left <= right:\n        mid = left + (right - left) // 2  # avoids integer overflow\n        if arr[mid] == target:\n            return mid\n        elif arr[mid] < target:\n            left = mid + 1\n        else:\n            right = mid - 1\n    return -1  # not found\n\nPrecondition: the array MUST be sorted. Binary search on an unsorted array gives wrong results.\n\nTime: O(log n) — 1 billion elements → at most 30 comparisons\nSpace: O(1) iterative, O(log n) recursive\n\nApplications beyond simple search:\n• Find first/last occurrence of a value\n• Find the smallest element ≥ target (lower_bound)\n• Search in rotated sorted array\n• Optimisation problems (binary search on the answer)\n\nCommon mistakes:\n• Off-by-one errors in boundary conditions\n• Infinite loop when mid doesn't change (fix: mid = left + (right-left)//2)",
                "Algorithms", "Beginner", "searching,binary-search,sorted-arrays,log-n", 7
            ),
            (
                "How would you reverse a linked list?",
                "Iterative approach — O(n) time, O(1) space:\n\ndef reverse_list(head):\n    prev, curr = None, head\n    while curr:\n        next_node = curr.next  # save next\n        curr.next = prev       # reverse pointer\n        prev = curr            # advance prev\n        curr = next_node       # advance curr\n    return prev  # new head\n\nStep-by-step on 1→2→3→None:\n• prev=None, curr=1: next=2, 1→None, prev=1, curr=2\n• prev=1,    curr=2: next=3, 2→1,    prev=2, curr=3\n• prev=2,    curr=3: next=None, 3→2, prev=3, curr=None\n→ return 3 (new head), list is 3→2→1→None\n\nRecursive approach — O(n) time, O(n) space (call stack):\ndef reverse_list(head):\n    if not head or not head.next:\n        return head\n    new_head = reverse_list(head.next)\n    head.next.next = head\n    head.next = None\n    return new_head\n\nFollow-up: reverse a sub-list (between positions m and n) — split into three parts, reverse the middle, reconnect.",
                "Algorithms", "Intermediate", "linked-list,reversal,pointers,iteration", 8
            ),

            // ════════════════════════════════════════════════════════
            // BEHAVIORAL
            // ════════════════════════════════════════════════════════
            (
                "Tell me about a challenging technical problem you solved.",
                "Use the STAR framework for this answer:\n\nSituation: Set the scene briefly. What project were you working on? What was the context?\nExample: 'During a production deployment, our payment service began timing out intermittently under load.'\n\nTask: What was your specific responsibility?\nExample: 'I was responsible for diagnosing and fixing the issue without taking the service offline.'\n\nAction: What did you actually do? Be specific about YOUR contributions:\n• How you approached debugging (logs, profiling, metrics)\n• What hypotheses you formed and tested\n• Tools you used\n• How you collaborated with the team\nExample: 'I used profiling tools to trace slow queries, identified an N+1 query pattern, then refactored the repository layer and added database indexes.'\n\nResult: Quantify the outcome.\nExample: 'Response times dropped from 3 seconds to 80ms. The fix shipped with zero downtime.'\n\nTips:\n• Prepare 2-3 real stories you can adapt to different questions\n• Show growth: mention what you learned\n• Don't make the story too complex — clarity wins",
                "Behavioral", "Beginner", "star-method,problem-solving,storytelling", 1
            ),
            (
                "How do you handle disagreements with teammates about technical decisions?",
                "Strong answer structure:\n\n1. Listen first: explain that you start by genuinely understanding their reasoning, not just waiting for your turn to talk. Ask clarifying questions.\n\n2. Separate technical from personal: focus on the trade-offs of each approach, not on 'winning' the argument.\n\n3. Bring data: prototype both approaches, benchmark performance, or refer to industry best practices. Objective evidence beats opinions.\n\n4. Find common ground: identify the shared goal and evaluate solutions against it.\n\n5. Know when to defer: if it's not a critical decision and the colleague has more context, defer to their judgment. Choose the relationship over being right.\n\n6. Escalate appropriately: for truly important architectural decisions where consensus can't be reached, involve a tech lead or run a team RFC (Request for Comments).\n\nExample answer: 'In my last project, I disagreed with a teammate about whether to use a monolith or microservices. Instead of arguing, I wrote a one-pager outlining the trade-offs for our specific scale and timeline. We discussed it as a team and agreed a modular monolith was the right starting point. I've found that written proposals depersonalise the debate and create a record.'",
                "Behavioral", "Intermediate", "teamwork,conflict-resolution,communication", 2
            ),
            (
                "How do you prioritise tasks when everything feels urgent?",
                "A structured approach:\n\n1. Clarify what 'urgent' actually means: ask stakeholders for deadlines and impact. Often 'urgent' means 'I'd like it soon' not 'the business breaks without it today.'\n\n2. Use a prioritisation framework:\n   • Eisenhower Matrix: Urgent+Important (do now), Important+Not Urgent (schedule), Urgent+Not Important (delegate), Neither (eliminate)\n   • RICE scoring: Reach × Impact × Confidence ÷ Effort\n\n3. Communicate proactively: if two tasks genuinely can't both be done, tell the stakeholders and let them decide the priority. Never silently drop tasks.\n\n4. Time-box: allocate focused blocks. Don't context-switch every hour.\n\n5. Review regularly: at the start of each day, re-rank your list — priorities shift.\n\nExample: 'When I have competing priorities, I first make a list with rough effort estimates and deadlines. I share it with my manager and ask: given these constraints, which should I tackle first? This gives visibility and transfers the priority decision to the right person while keeping me unblocked.'",
                "Behavioral", "Beginner", "time-management,prioritisation,productivity", 3
            ),
            (
                "Tell me about a project you're proud of.",
                "This question tests your passion, ownership, and ability to reflect on your work. Use it to showcase skills relevant to the role.\n\nStructure your answer:\n1. Context: what was the project? What problem did it solve?\n2. Your role: what were you personally responsible for?\n3. Technical highlights: mention interesting decisions — architecture choices, challenges overcome, technologies used\n4. Results: impact — users helped, performance improved, time saved, revenue generated\n5. What you're proud of: the specific aspect that you'd do the same again + one thing you'd do differently\n\nTips:\n• Choose a project that's relevant to the job you're applying for\n• Show ownership: 'I built', 'I designed', 'I led' — not just 'we'\n• Quantify: '10,000 monthly active users', '40% faster load time', 'reduced deployment time from 2 hours to 8 minutes'\n• Show learning and growth — interviewers value reflection\n• If you're a student, academic or personal projects count equally",
                "Behavioral", "Beginner", "projects,ownership,impact,passion", 4
            ),
            (
                "How do you stay up to date with technology?",
                "Interviewers want to see genuine curiosity and a sustainable learning practice — not just a list of buzzwords.\n\nGood answer elements:\n\n1. Specific sources you actually use:\n   • Reading: Hacker News, dev.to, engineering blogs (Netflix Tech Blog, Stripe Blog, Martin Fowler, Dan Abramov)\n   • Podcasts: Syntax, Software Engineering Daily, The Changelog\n   • Newsletters: TLDR, JavaScript Weekly, Python Weekly\n   • Papers: ACM, arXiv for deeper CS topics\n\n2. Hands-on practice:\n   • Side projects that use new technologies\n   • Contributing to open source\n   • LeetCode, Codewars for algorithm skills\n   • Building small tools to solve real problems\n\n3. Community:\n   • Conferences (online): Google I/O, WWDC, PyCon, JSConf\n   • Local meetups\n   • Twitter/X tech community\n\n4. Depth over breadth:\n   • Show you go beyond tutorials: 'I read the RFC when async/await was proposed'\n   • Talk about what you experimented with recently and what you concluded\n\nHonest acknowledgement: 'I don't chase every trend — I focus on fundamentals and pick up new tools when a project requires it.'",
                "Behavioral", "Beginner", "learning,growth,curiosity,professional-development", 5
            ),
            (
                "Describe your experience with code reviews — giving and receiving.",
                "Giving code reviews:\n• Be specific and actionable: 'This function does three things — consider splitting it' is better than 'messy code'\n• Distinguish blocking issues (bugs, security) from suggestions (style, naming) — label them clearly\n• Ask questions rather than make demands: 'Could we handle the null case here?' instead of 'This is wrong'\n• Acknowledge good work — don't only comment on problems\n• Review the design and logic first, then details; don't nitpick style if a linter handles it\n\nReceiving code reviews:\n• Separate your code from your identity — criticism of code is not criticism of you\n• If you disagree, ask for the reasoning — 'Help me understand why you'd prefer this approach'\n• Apply the suggested change or explain why you won't — don't silently ignore feedback\n• Thank reviewers for thorough reviews — it's unpaid work\n\nExample: 'I try to front-load the review with a short summary of what the PR does so reviewers can focus on logic instead of reverse-engineering it. On the receiving end, I treat every comment as a chance to learn something I missed.'",
                "Behavioral", "Intermediate", "code-review,collaboration,feedback,teamwork", 6
            ),
            (
                "Where do you see yourself in 5 years?",
                "Interviewers aren't looking for a rigid career plan — they want to see ambition, self-awareness, and alignment with the role.\n\nWhat makes a strong answer:\n1. Honest direction (not vague platitudes): 'I'd like to grow into a senior/lead role with deeper expertise in distributed systems' is better than 'I just want to learn'\n2. Alignment with the company: show that your goals and the company's trajectory overlap — you'll both benefit from the relationship\n3. Skills focus: talk about capabilities you want to develop (technical leadership, system design, a specific domain)\n4. Flexibility: acknowledge that paths change — what matters is continuous growth\n\nExample: 'In 5 years I'd like to be a technical lead — someone who can architect systems end-to-end and mentor junior developers. I'm drawn to this role because it's in a team doing [relevant work], which aligns with where I want to develop depth. I also see this company expanding into [relevant area], and I'd want to be part of that growth.'\n\nWhat to avoid:\n• 'I want your job' — usually comes across as awkward\n• 'I'd like to start my own company in 2 years' — signals low commitment\n• Vague non-answers like 'just keep learning and see what happens'",
                "Behavioral", "Beginner", "career-goals,ambition,self-awareness", 7
            ),
            (
                "Tell me about a time you failed or made a mistake.",
                "This is one of the most important behavioural questions — it reveals self-awareness, accountability, and resilience.\n\nWhat interviewers want to see:\n• You take ownership (no blame-shifting)\n• You learned something concrete from it\n• You changed your behaviour as a result\n\nUse the STAR + L framework (Situation, Task, Action, Result + Learning):\n\nSituation: 'During a major product release, I merged a database migration that I had tested locally but not in staging.'\n\nTask: 'I was responsible for the deployment pipeline.'\n\nAction: 'The migration caused data corruption in production. I immediately rolled back, communicated the issue to the team, and worked through the night to restore data from backups.'\n\nResult: 'We recovered all data within 4 hours with no permanent loss. Users were notified with a transparent status page post.'\n\nLearning: 'I now enforce a staging promotion gate for all migrations and require peer review of migration scripts specifically.'\n\nTips:\n• Pick a real story — fabricated stories collapse under follow-up questions\n• Choose a mistake that was genuinely meaningful, not trivial\n• The learning must be concrete and behavioural — not just 'I'll be more careful'",
                "Behavioral", "Intermediate", "failure,accountability,growth,resilience", 8
            ),
        };

        foreach (var (q, a, cat, diff, tags, order) in questions)
        {
            context.InterviewQuestions.Add(new InterviewQuestion
            {
                Id          = Guid.NewGuid(),
                Question    = q,
                ModelAnswer = a,
                Category    = cat,
                Difficulty  = diff,
                Tags        = tags,
                OrderNumber = order
            });
        }

        await context.SaveChangesAsync();
    }
}
