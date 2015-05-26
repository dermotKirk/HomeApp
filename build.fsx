#r @"packages/FAKE/tools/FakeLib.dll"
#load "build-helpers.fsx"

open Fake
open BuildHelpers

Target "common-build" (fun () ->
    RestorePackages "TodoPCL.sln"

    MSBuild "Todo/bin/Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ] [ "TodoPCL.sln" ] |> ignore
)

Target "common-tests" (fun () ->
    RunNUnitTests "Todo/bin/Debug/TodoPCL.Tests.dll"
    "Todo/bin/Debug/testresults.xml" |> ignore
)

"common-build"
  ==> "common-tests"

RunTargetOrDefault "common-tests"