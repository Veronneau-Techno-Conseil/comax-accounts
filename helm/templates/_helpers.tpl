{{/*
Expand the name of the secret.
*/}}
{{- define "comax-accounts.secretName" -}}
{{- printf "%s-secrets" ( include "comax-accounts.name" . ) -}}
{{- end }}

{{/*
Expand the full service name.
*/}}
{{- define "comax-accounts.fullSvcName" -}}
{{- printf "%s.%s.svc.cluster.local" ( include "comax-accounts.name" . ) .Release.Namespace -}}
{{- end }}

{{/*
Expand the name of the cert secret.
*/}}
{{- define "comax-accounts.certSecretName" -}}
{{- printf "%s-depltls" ( include "comax-accounts.name" . ) -}}
{{- end }}

{{/*
Expand the name of the chart.
*/}}
{{- define "comax-accounts.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "comax-accounts.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "comax-accounts.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "comax-accounts.labels" -}}
helm.sh/chart: {{ include "comax-accounts.chart" . }}
{{ include "comax-accounts.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "comax-accounts.selectorLabels" -}}
app.kubernetes.io/name: {{ include "comax-accounts.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "comax-accounts.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "comax-accounts.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
