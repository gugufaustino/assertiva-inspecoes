
@echo off
set v_versao=%~1
set /A i_versao = 0 
set /A i_versao = (v_versao + 1)

@echo Versao Atual: %v_versao%   
@echo Nova Versao: %i_versao%  
@echo ##vso[task.setvariable variable=appversao;]v%i_versao%
echo ##vso[task.setvariable variable=appversao;] v%i_versao%