#r @"packages/FAKE/tools/FakeLib.dll"

open Fake

Target "common-build" (fun () ->

)

Target "common-tests" (fun () ->

)

"common-build"
  ==> "common-tests"

RunTargetOrDefault "common-tests"