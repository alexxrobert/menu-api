#!/bin/bash

solution_dir="src"
api_project_dir="Storefront.Menu.API"
test_project_dir="Storefront.Menu.Tests"

set -e

dotnet clean $solution_dir

rm -rf $solution_dir/$api_project_dir/bin
rm -rf $solution_dir/$api_project_dir/obj
rm -rf $solution_dir/$test_project_dir/bin
rm -rf $solution_dir/$test_project_dir/obj
rm -rf $solution_dir/$test_project_dir/coverage

dotnet restore $solution_dir
